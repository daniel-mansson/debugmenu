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
using JsonSerializerSettings = Newtonsoft.Json.JsonSerializerSettings;
using NullValueHandling = Newtonsoft.Json.NullValueHandling;

namespace DebugMenu {
    public class DebugMenuClient : IDisposable {
        private readonly string _url;
        private readonly string _token;
        private readonly Dictionary<string, string> _metadata;

        private DebugMenuWebSocketClient? _webSocketClient;
        private Task? _clientTask;

        private readonly Dictionary<string, DebugMenuChannelHandler> _handlers2 = new();

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
                Channels = _handlers2.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.GetSchema())
            };

            var settings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(document, settings);
        }

        private void OnReceivedJson((string channel, JObject payload) message) {
            if(_handlers2.TryGetValue(message.channel.ToLowerInvariant(), out var handler)) {
                var returnValue = handler.HandleMessage(message.payload);
                if(returnValue != null) {
                    Debug.Log($"Return value: {returnValue}");
                    _webSocketClient?.SendJson(message.channel, returnValue, CancellationToken.None);
                }
            }
        }

        public void Dispose() {
            _webSocketClient?.Dispose();
            _webSocketClient = null;
            _clientTask = null;
        }

        public void RegisterController(object controller) {
            var type = controller.GetType();

            var controllerAttribute = type.GetCustomAttribute<ControllerAttribute>();
            if(controllerAttribute == null) {
                return;
            }

            var handlers = RegisterMethods(controller, type);
            foreach(var handler in handlers) {
                _handlers2.Add(handler.Channel.ToLowerInvariant(), handler);
            }

            TryUpdateSchema();
        }

        private List<DebugMenuChannelHandler> RegisterMethods(object controller, Type type) {
            var handlers = new List<DebugMenuChannelHandler>();
            var methods = type.GetMethods();

            foreach(var methodInfo in methods) {
                var buttonAttr = methodInfo.GetCustomAttribute<ButtonAttribute>();
                if(buttonAttr != null) {
                    handlers.Add(new ButtonDebugMenuChannelHandler(GetChannel(controller, methodInfo), controller, methodInfo, buttonAttr));
                    continue;
                }

                var toggleAttr = methodInfo.GetCustomAttribute<ToggleAttribute>();
                if(toggleAttr != null) {
                    handlers.Add(new ToggleDebugMenuChannelHandler(GetChannel(controller, methodInfo), controller, methodInfo, toggleAttr));
                }
            }

            return handlers;
        }

        private string GetChannel(object instance, MethodInfo methodInfo) {
            var type = instance.GetType();
            var controllerAttribute = type.GetCustomAttribute<ControllerAttribute>();
            var methodAttribute = methodInfo.GetCustomAttribute<DebugMenuChannelAttribute>();

            return $"{controllerAttribute?.Path ?? type.Name}/{methodAttribute?.Path ?? methodInfo.Name}";
        }
    }
}
