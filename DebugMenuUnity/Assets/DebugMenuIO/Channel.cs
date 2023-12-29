#nullable enable
using System;
using System.Threading;

public class Channel : IChannel {
    private readonly DebugMenuWebSocketClient _client;
    private readonly string _channelPath;
    private readonly CancellationToken _cancellationToken;

    public Channel(DebugMenuWebSocketClient client, string channelPath, CancellationToken cancellationToken) {
        _client = client;
        _channelPath = channelPath;
        _cancellationToken = cancellationToken;

        _client.ReceivedJson += OnReceivedJson;
        _client.ReceivedBytes += OnReceivedBytes;
    }

    private void OnReceivedBytes((string channel, ReadOnlyMemory<byte> payload) message) {
        if (message.channel == _channelPath) {
            ReceivedBytes?.Invoke(message.payload);
        }
    }

    private void OnReceivedJson((string channel, Unity.Plastic.Newtonsoft.Json.Linq.JObject payload) message) {
        if (message.channel == _channelPath) {
            ReceivedJson?.Invoke(message.payload);
        }
    }

    public void SendJson(object payload) {
        _client.SendJson(_channelPath, payload, _cancellationToken);
    }

    public void SendBinary(ReadOnlyMemory<byte> bytes) {
        _client.SendBytes(_channelPath, bytes, _cancellationToken);
    }

    public event Action<Unity.Plastic.Newtonsoft.Json.Linq.JObject>? ReceivedJson;
    public event Action<ReadOnlyMemory<byte>>? ReceivedBytes;
}