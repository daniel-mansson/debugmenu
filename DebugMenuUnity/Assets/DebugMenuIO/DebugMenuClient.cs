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
using DebugMenuIO.AsyncApi;
using Newtonsoft.Json.Serialization;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.XR;
using Channel = DebugMenuIO.AsyncApi.Channel;
using JsonSerializerSettings = Newtonsoft.Json.JsonSerializerSettings;
using NullValueHandling = Newtonsoft.Json.NullValueHandling;

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

        public async Task<RunningInstance> Run(CancellationToken cancellationToken) {
            if(_clientTask != null) {
                throw new InvalidOperationException($"{nameof(DebugMenuClient)} is already running.");
            }

            var body = JsonConvert.SerializeObject(new CreateRunningInstanceRequest() {
                Token = _token,
                Metadata = _metadata
            });
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(_url + "/api/instances", content, cancellationToken);

            var responseJson = await response.Content.ReadAsStringAsync();
            var instance = JsonConvert.DeserializeObject<RunningInstance>(responseJson)!;

            _webSocketClient =
                new DebugMenuWebSocketClient(instance.WebsocketUrl! + "/instance", _token, _metadata, OnConnected);

            _webSocketClient.ReceivedJson += OnReceivedJson;
            _webSocketClient.ErrorOccurred += OnError;

            _clientTask = _webSocketClient.Run();

            return instance;
        }

        private void OnError(Exception ex) {
            Debug.LogException(ex);
        }

        private void TryUpdateSchema() {
            if(_webSocketClient != null) {
                var api = BuildApi();

                Debug.Log(api);

                var _ = _webSocketClient.SendBytes("__internal/api",
                    Encoding.UTF8.GetBytes(api),
                    CancellationToken.None);
            }
        }

        private string BuildApi() {
            var document = new Document() {
                Asyncapi = "2.6.0",
                Info = new Info() {
                    Title = "Test",
                    Version = "1.0.0"
                },
                Channels = _handlers.ToDictionary(
                    kvp => kvp.Key,
                    kvp => BuildChannel(kvp.Value))
            };

            var settings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(document, settings);
        }

        private Channel BuildChannel(HandlerInfo handlerInfo) {
            var parameters = handlerInfo.Method.GetParameters();

            return new Channel() {
                Publish = new Publish() {
                    Tags = new List<Tag>() {
                        new Tag() {
                            Name = "button"
                        }
                    },
                    Message = new Message() {
                        Payload = new Payload() {
                            Type = "object",
                            Properties = parameters.Select(p => {
                                return (p.Name, new Property() {
                                    Type = GetPropertyType(p.ParameterType),
                                });
                            }).ToDictionary(p => p.Name, p => p.Item2)
                        }
                    }
                }
            };
        }

        private string GetPropertyType(Type type) {
            if(type == typeof(int) || type == typeof(float) || type == typeof(double)) {
                return "number";
            }

            if(type == typeof(string)) {
                return "string";
            }

            return "unknown";
        }

        private void OnReceivedJson((string channel, JObject payload) message) {
            Debug.Log($"R: {message.channel} {message.payload.ToString()}");
            if(_handlers.TryGetValue(message.channel.ToLowerInvariant(), out var handler)) {
                var parameters = handler.Method.GetParameters()
                    .Select(p => ToValue(message.payload, p))
                    .ToArray();

                handler.Method.Invoke(handler.Instance, parameters);
            }
        }

        private static object? ToValue(JObject payload, ParameterInfo p) {
            var property = payload[p.Name];
            if(property == null) {
                return null;
            }

            if(p.ParameterType == typeof(int)) {
                return property.Value<int>();
            }
            if(p.ParameterType == typeof(long)) {
                return property.Value<long>();
            }
            if(p.ParameterType == typeof(float)) {
                return property.Value<float>();
            }
            if(p.ParameterType == typeof(double)) {
                return property.Value<double>();
            }
            if(p.ParameterType == typeof(string)) {
                return property.Value<string>();
            }

            return property.Value<object>();
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

            var methods = type.GetMethods()
                .Where(m => m.GetCustomAttribute<ButtonAttribute>() != null)
                .ToList();

            foreach(var method in methods) {
                RegisterHandler(controller, method);
            }

            TryUpdateSchema();
        }

        public void RegisterHandler(object instance, MethodInfo methodInfo) {
            var channel = GetChannel(instance, methodInfo);
            var id = channel.ToLowerInvariant();

            if(_handlers.ContainsKey(id)) {
                return;
            }

            var handler = new HandlerInfo() {
                Channel = channel,
                Instance = instance,
                Method = methodInfo
            };
            _handlers.Add(id, handler);
        }

        private class HandlerInfo {
            public string Channel { get; set; }
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
