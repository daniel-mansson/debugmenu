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
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine.XR;

namespace DebugMenu {
    public class DebugMenuClient : IDisposable {
        private readonly string _url;
        private readonly string _token;
        private readonly Dictionary<string, string> _metadata;
        private DebugMenuWebSocketClient? _webSocketClient;
        private Task? _clientTask;
        private readonly Dictionary<string, Channel> _channels = new();
        private readonly Dictionary<string, HandlerInfo> _handlers = new();

        public DebugMenuClient(string url, string token, Dictionary<string, string> metadata) {
            _url = url;
            _token = token;
            _metadata = metadata;
        }

        private Task OnConnected() {
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

            _clientTask = _webSocketClient.Run();

            return instance;
        }

        private void TryUpdateSchema() {
            if(_webSocketClient != null) {
                var api = BuildApi();

                var _ = _webSocketClient.SendBytes("__internal/api",
                    Encoding.UTF8.GetBytes(api),
                    CancellationToken.None);
            }
        }

        private string BuildApi() {
            throw new NotImplementedException();
        }

        private void OnReceivedJson((string channel, JObject payload) message) {
            if(_handlers.TryGetValue(message.channel, out var handler)) {
                var parameters = handler.Method.GetParameters()
                    .Select(p => message.payload[p.Name]?.Value<object>())
                    .ToArray();

                handler.Method.Invoke(handler.Instance, parameters);
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
                channel = new Channel(_webSocketClient, channelPath, CancellationToken.None); //TODO cancellation token
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
        }

        public void RegisterHandler(object instance, MethodInfo methodInfo) {
            var channel = GetChannel(instance, methodInfo);

            if(_handlers.ContainsKey(channel)) {
                return;
            }

            var handler = new HandlerInfo() {
                Channel = channel,
                Instance = instance,
                Method = methodInfo
            };
            _handlers.Add(channel, handler);
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
