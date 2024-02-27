#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DebugMenuWebSocketClient : ISender, IReceiver, IDisposable {
    private readonly string _url;
    private readonly string _token;
    private readonly Dictionary<string, string> _metadata;
    private ClientWebSocket _socket;

    private readonly CancellationTokenSource _disposeCancellationTokenSource = new();
    private readonly Func<Task>? _connectedAfterHandshakeCallback;
    private readonly CancellationToken _disposedCancellationToken;

    [Serializable]
    public class Message {
        public string? channel;
        public object? payload;
    }

    public DebugMenuWebSocketClient(string url, string token, Dictionary<string, string> metadata,
        Func<Task> connectedAfterHandshakeCallback) {
        _url = url;
        _token = token;
        _metadata = metadata;
        _connectedAfterHandshakeCallback = connectedAfterHandshakeCallback;
        _socket = new ClientWebSocket();
        _disposedCancellationToken = _disposeCancellationTokenSource.Token;
    }

    public bool IsConnected => IsSocketAlive();

    public Task SendJson(string channel, object payload, CancellationToken cancellationToken) {
        if(!IsSocketAlive()) {
            return Task.CompletedTask;
        }

        var jsonString = JsonConvert.SerializeObject(new Message() {
            channel = channel,
            payload = payload
        });
        var bytes = Encoding.UTF8.GetBytes(jsonString);

        var cts = CancellationTokenSource.CreateLinkedTokenSource(_disposedCancellationToken, cancellationToken);
        return _socket.SendAsync(bytes, WebSocketMessageType.Text, true, cts.Token);
    }

    public Task SendBytes(string channel, ReadOnlyMemory<byte> payload, CancellationToken cancellationToken) {
        if(!IsSocketAlive()) {
            return Task.CompletedTask;
        }

        var channelBytes = Encoding.UTF8.GetBytes(channel);
        if(channelBytes.Length > byte.MaxValue) {
            throw new Exception($"Channel name too long. Length is {channelBytes.Length}, max is {byte.MaxValue}.");
        }

        var bytes = new byte[payload.Length + 1 + channelBytes.Length];
        var stream = new MemoryStream(bytes);
        var writer = new BinaryWriter(stream);

        writer.Write((byte)channelBytes.Length);
        writer.Write(channelBytes);
        writer.Write(payload.Span);

        var cts = CancellationTokenSource.CreateLinkedTokenSource(_disposedCancellationToken, cancellationToken);
        return _socket.SendAsync(bytes, WebSocketMessageType.Binary, true, cts.Token);
    }

    public event Action<(string channel, JObject payload)>? ReceivedJson;
    public event Action<(string channel, ReadOnlyMemory<byte> payload)>? ReceivedBytes;
    public event Action<Exception>? ErrorOccurred;

    public async Task Run() {
        var buffer = new byte[4096 * 20];
        var stream = new MemoryStream(buffer);

        var expandableStream = new MemoryStream();
        var writer = new BinaryWriter(expandableStream);

        while(!_disposedCancellationToken.IsCancellationRequested) {
            try {
                await TryReconnect(_disposedCancellationToken);

                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), _disposedCancellationToken);

                if(result.EndOfMessage) {
                    var streamToUse = expandableStream.Position == 0 ? stream : expandableStream;

                    switch(result.MessageType) {
                    case WebSocketMessageType.Binary:
                        HandleBinaryMessage(streamToUse, buffer, result);
                        break;
                    case WebSocketMessageType.Text:
                        HandleTextMessage(streamToUse, buffer, result);
                        break;
                    }

                    expandableStream.Seek(0, SeekOrigin.Begin);
                }
                else {
                    writer.Write(buffer, 0, result.Count);
                }
            }
            catch(OperationCanceledException e) {
                return;
            }
            catch(Exception e) {
                ErrorOccurred?.Invoke(e);
            }

            if(!IsSocketAlive()) {
                UnityEngine.Debug.Log($"Disconnected {_url}");
                await Task.Delay(2000, _disposedCancellationToken);
            }
        }
    }

    private async Task TryReconnect(CancellationToken cancellationToken) {
        if(!IsSocketAlive()) {
            _socket = new ClientWebSocket();
            await _socket.ConnectAsync(new Uri(_url), cancellationToken);

            var tokenBytes = Encoding.UTF8.GetBytes(_token);
            await _socket.SendAsync(tokenBytes, WebSocketMessageType.Text, true, cancellationToken);

            var metadataJson = JsonConvert.SerializeObject(_metadata);
            var metadataBytes = Encoding.UTF8.GetBytes(metadataJson);
            await _socket.SendAsync(metadataBytes, WebSocketMessageType.Text, true, cancellationToken);

            var task = _connectedAfterHandshakeCallback?.Invoke();
            if(task != null) {
                await task;
            }

            UnityEngine.Debug.Log($"Connected {_url}");
        }
    }

    private bool IsSocketAlive() {
        return _socket is { CloseStatus: null } && _socket.State != WebSocketState.Aborted
                                                && _socket.State != WebSocketState.None
                                                && _socket.State != WebSocketState.Closed
                                                && _socket.State != WebSocketState.Connecting;
    }

    private void HandleTextMessage(MemoryStream stream, byte[] buffer, WebSocketReceiveResult result) {
        stream.Seek(0, SeekOrigin.Begin);
        var text = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(buffer, 0, result.Count));
        try {
            var document = JObject.Parse(text);
            var channel = document.GetValue("channel")!.Value<string>();
            var payload = document.GetValue("payload")!.Value<JObject>();
            ReceivedJson?.Invoke((channel!, payload!));
        }
        catch(Exception) {
            UnityEngine.Debug.LogError($"Exception while parsing: {text}");
            throw;
        }
    }

    private void HandleBinaryMessage(MemoryStream stream, byte[] buffer,
        WebSocketReceiveResult result) {
        stream.Seek(0, SeekOrigin.Begin);
        var reader = new BinaryReader(stream);
        var channelLength = reader.ReadByte();
        var channel = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(buffer, 1, channelLength));
        var payload = new ReadOnlyMemory<byte>(buffer, 1 + channelLength, result.Count - 1 - channelLength);
        ReceivedBytes?.Invoke((channel, payload));
    }

    public void Dispose() {
        _disposeCancellationTokenSource.Cancel();
        _socket.Dispose();
    }
}
