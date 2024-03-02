#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DebugMenuIO;
using DebugMenuIO.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using Channel = DebugMenuIO.Schema.Channel;
using JsonSerializerSettings = Newtonsoft.Json.JsonSerializerSettings;
using NullValueHandling = Newtonsoft.Json.NullValueHandling;
using Payload = DebugMenuIO.Schema.Payload;
using Property = DebugMenuIO.Schema.Property;

namespace DebugMenu {
    public class DebugMenuClient : IDisposable {
        private readonly string _url;
        private readonly string _token;
        private readonly Dictionary<string, string> _metadata;
        private DebugMenuWebSocketClient? _webSocketClient;
        private Task? _clientTask;
        private readonly Dictionary<string, ClientChannel> _channels = new();
        private readonly Dictionary<string, HandlerInfo> _handlers = new();

        public DebugMenuClient(string url, string token, Dictionary<string, string> metadata) {
            _url = url;
            _token = token;
            _metadata = metadata;
        }

        private Task OnConnected() {
            TryUpdateSchema();
            return Task.CompletedTask;
        }


        [Serializable]
        private class CreateRunningInstanceRequest {
            public string Token { get; set; } = string.Empty;
            public Dictionary<string, string> Metadata { get; set; } = new();
        }

        [Serializable]
        public class RunningInstance {
            public string Id { get; set; } = string.Empty;
            public string? DeviceId { get; set; }
            public string? WebsocketUrl { get; set; }
            public int ConnectedViewers { get; set; }
            public bool HasConnectedInstance { get; set; }
            public int ApplicationId { get; set; }
        }

        public async Task<RunningInstance> Run(CancellationToken cancellationToken,
            RunningInstance? runningInstance = null) {
            if(_clientTask != null) {
                throw new InvalidOperationException($"{nameof(DebugMenuClient)} is already running.");
            }

            if(!await IsValidInstance(runningInstance)) {
                runningInstance = await RequestInstance(cancellationToken);
            }

            _webSocketClient =
                new DebugMenuWebSocketClient(runningInstance.WebsocketUrl! + "/instance", _token, _metadata,
                    OnConnected);

            _webSocketClient.ReceivedJson += OnReceivedJson;
            _webSocketClient.ErrorOccurred += OnError;

            _clientTask = _webSocketClient.Run();

            return runningInstance;
        }

        private async Task<bool> IsValidInstance(RunningInstance? runningInstance) {
            return runningInstance != null;
        }

        private async Task<RunningInstance> RequestInstance(CancellationToken cancellationToken) {
            var body = JsonConvert.SerializeObject(new CreateRunningInstanceRequest() {
                Token = _token,
                Metadata = _metadata
            });
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var client = new HttpClient();

            Debug.Log($"Requesting instance");
            var response = await client.PostAsync(_url + "/api/instances", content, cancellationToken);

            var responseJson = await response.Content.ReadAsStringAsync();
            Debug.Log($"Got instance {responseJson}");
            var i = JsonConvert.DeserializeObject<RunningInstance>(responseJson)!;
            return i;
        }

        private void OnError(Exception ex) {
            Debug.LogException(ex);
        }

        private void TryUpdateSchema() {
            if(_webSocketClient != null && _webSocketClient.IsConnected) {
                var api = BuildApi();

                Debug.Log(api);

                var _ = _webSocketClient.SendBytes("__internal/api",
                    Encoding.UTF8.GetBytes(api),
                    CancellationToken.None);
            }
        }

        private string BuildApi() {
            var document = new Api() {
                DebugMenuApi = "1.0.0",
                Channels = _handlers.ToDictionary(
                    kvp => kvp.Key,
                    kvp => BuildChannel(kvp.Value))
            };

            var settings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(document, settings);
        }

        private Channel BuildChannel(HandlerInfo handlerInfo) {
            var parameters = handlerInfo.Method.GetParameters();

            if(handlerInfo.Type == "button") {
                return new Channel() {
                    Type = handlerInfo.Type,
                    Publish = new Payload() {
                        Type = "object",
                        Properties = parameters.Select(p => {
                            return (p.Name, new Property() {
                                Type = DebugMenuUtils.GetPropertyType(p.ParameterType)
                            });
                        }).ToDictionary(p => p.Name, p => p.Item2)
                    }
                };
            }
            else if(handlerInfo.Type == "toggle") {
                return new Channel() {
                    Type = handlerInfo.Type,
                    Publish = new Payload() {
                        Type = "object",
                        Properties = new Dictionary<string, Property> {
                            {
                                "value", new Property {
                                    Type = "boolean"
                                }
                            }
                        }
                    },
                    Subscribe = new Payload() {
                        Type = "object",
                        Properties = new Dictionary<string, Property> {
                            {
                                "value", new Property {
                                    Type = "boolean"
                                }
                            }
                        }
                    }
                };
            }

            return new Channel() {
                Type = handlerInfo.Type
            };
        }

        private void OnReceivedJson((string channel, JObject payload) message) {
            if(_handlers.TryGetValue(message.channel.ToLowerInvariant(), out var handler)) {
                var parameters = handler.Method.GetParameters()
                    .Select(p => DebugMenuUtils.ToValue(message.payload, p))
                    .ToArray();

                var returnValue = handler.Method.Invoke(handler.Instance, parameters);
                if(returnValue != null) {
                    Debug.Log($"Return value: {returnValue}");
                    _webSocketClient?.SendJson(message.channel, new { value = returnValue }, CancellationToken.None);
                }
            }
        }

        public void Dispose() {
            _webSocketClient?.Dispose();
            _webSocketClient = null;
            _clientTask = null;
        }

        public IChannel Channel(string channelPath) {
            if(_webSocketClient == null) {
                throw new Exception($"{nameof(DebugMenuClient)} not yet initialized.");
            }

            if(!_channels.TryGetValue(channelPath, out var channel)) {
                channel = new ClientChannel(_webSocketClient, channelPath,
                    CancellationToken.None); //TODO cancellation token
                _channels.Add(channelPath, channel);
            }

            return channel;
        }

        public void RegisterHandler(Action reset) {
        }

        public void RegisterHandler<T>(Action<T> setGold) {
        }

        public void RegisterController(object controller) {
            var type = controller.GetType();

            var controllerAttribute = type.GetCustomAttribute<ControllerAttribute>();
            if(controllerAttribute == null) {
                return;
            }

            RegisterButtons(controller, type);
            RegisterToggles(controller, type);

            TryUpdateSchema();
        }

        private void RegisterButtons(object controller, Type type) {
            var methods = type.GetMethods()
                .Where(m => m.GetCustomAttribute<ButtonAttribute>() != null)
                .ToList();
            foreach(var method in methods) {
                RegisterHandler(controller, method, "button");
            }
        }

        private void RegisterToggles(object controller, Type type) {
            var methods = type.GetMethods()
                .Where(m => m.GetCustomAttribute<ToggleAttribute>() != null)
                .ToList();
            foreach(var method in methods) {
                RegisterHandler(controller, method, "toggle");
            }
        }

        public void RegisterHandler(object instance, MethodInfo methodInfo, string type) {
            var channel = GetChannel(instance, methodInfo);
            var id = channel.ToLowerInvariant();

            if(_handlers.ContainsKey(id)) {
                return;
            }

            var handler = new HandlerInfo() {
                Channel = channel,
                Instance = instance,
                Method = methodInfo,
                Type = type
            };
            _handlers.Add(id, handler);
        }

        private class HandlerInfo {
            public string Channel { get; set; }
            public string Type { get; set; }
            public object Instance { get; set; }
            public MethodInfo Method { get; set; }
        }

        private string GetChannel(object instance, MethodInfo methodInfo) {
            var type = instance.GetType();
            var controllerAttribute = type.GetCustomAttribute<ControllerAttribute>();
            var methodAttribute = methodInfo.GetCustomAttribute<ButtonAttribute>();

            return $"{controllerAttribute?.Path ?? type.Name}/{methodAttribute?.Path ?? methodInfo.Name}";
        }
    }
}
