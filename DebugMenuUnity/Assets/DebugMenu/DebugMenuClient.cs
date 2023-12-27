#nullable enable

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;

namespace DebugMenu
{
    public class DebugMenuClient : IDisposable {
        private readonly string _url;
        private readonly string _token;
        private readonly Dictionary<string, string> _metadata;
        private DebugMenuWebSocketClient? _webSocketClient;
        private Task? _clientTask;
        private readonly Dictionary<string, Channel> _channels = new();

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

            _clientTask = _webSocketClient.Run();

            return instance;
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
    }
}
