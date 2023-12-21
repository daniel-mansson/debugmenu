using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace DebugMenu.ProducerClientSdk; 

public class DebugMenuClient {
    private readonly string _url;
    private readonly string _token;
    private ClientWebSocket _socket;
    private Dictionary<string, string> _metadata;
    private string _api;

    public DebugMenuClient(string url, string token) {
        _url = url;
        _token = token;
        _socket = new ClientWebSocket();
    }

    public async Task Initialize(Dictionary<string, string> metadata, string api, CancellationToken ct) {
        _metadata = metadata;
        _api = api;
        await TryReconnect(ct);

        var _ = Task.Run(() => ReceiveLoop(ct), ct);
    }
    
    public Task SendJson(string channel, object payload, CancellationToken cancellationToken) {
        if (!IsSocketAlive()) {
            return Task.CompletedTask;
        }
        
        var jsonString = JsonSerializer.Serialize(new {
            channel, payload
        });
        var bytes = Encoding.UTF8.GetBytes(jsonString);
        return _socket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellationToken);
    }

    public Task SendBytes(string channel, ReadOnlyMemory<byte> payload, CancellationToken cancellationToken) {
        if (!IsSocketAlive()) {
            return Task.CompletedTask;
        }
        
        var channelBytes = Encoding.UTF8.GetBytes(channel);
        if (channelBytes.Length > byte.MaxValue) {
            throw new Exception($"Channel name too long. Length is {channelBytes.Length}, max is {byte.MaxValue}.");
        }

        var bytes = new byte[payload.Length + 1 + channelBytes.Length];
        var stream = new MemoryStream(bytes);
        var writer = new BinaryWriter(stream);
        
        writer.Write((byte)channelBytes.Length);
        writer.Write(channelBytes);
        writer.Write(payload.Span);
        
        return _socket.SendAsync(bytes, WebSocketMessageType.Binary, true, cancellationToken);
    }

    public event Action<(string channel, JsonElement payload)>? ReceivedJson;
    public event Action<(string channel, ReadOnlyMemory<byte> payload)>? ReceivedBytes;
    public event Action<Exception>? ErrorOccurred;

    private async Task TryReconnect(CancellationToken ct) {
        if (!IsSocketAlive()) {
            _socket = new ClientWebSocket();
            await _socket.ConnectAsync(new Uri(_url), ct);
            
            var tokenBytes = Encoding.UTF8.GetBytes(_token);
            await _socket.SendAsync(tokenBytes, WebSocketMessageType.Text, true, ct);

            var metadataJson = JsonSerializer.Serialize(_metadata);
            var metadataBytes = Encoding.UTF8.GetBytes(metadataJson);
            await _socket.SendAsync(metadataBytes, WebSocketMessageType.Text, true, ct);

            await SendBytes("__internal/api", Encoding.UTF8.GetBytes(_api), ct);
            
            Console.WriteLine($"Connected {_url}");
        }
    }

    private bool IsSocketAlive() {
        return _socket is { CloseStatus: null } && _socket.State !=  WebSocketState.Aborted
                                                && _socket.State !=  WebSocketState.None
                                                && _socket.State !=  WebSocketState.Closed;
    }

    private void HandleTextMessage(MemoryStream stream, byte[] buffer, WebSocketReceiveResult result) {
        stream.Seek(0, SeekOrigin.Begin);
        var text = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(buffer, 0, result.Count));
        var document = JsonDocument.Parse(text);
        var channel = document.RootElement.GetProperty("channel").GetString()!;
        var payload = document.RootElement.GetProperty("payload");
        ReceivedJson?.Invoke((channel, payload));
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
        _socket.Dispose();
    }

    private async Task ReceiveLoop(CancellationToken ct) {
        var buffer = new byte[4096 * 20];
        var stream = new MemoryStream(buffer);
       
        var expandableStream = new MemoryStream();
        var writer = new BinaryWriter(expandableStream);

        while (!_socket.CloseStatus.HasValue) {

            bool wasSocketAlive = false;
            try {
                await TryReconnect(ct);
                wasSocketAlive = IsSocketAlive();
                
                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.EndOfMessage) {
                    var streamToUse = expandableStream.Position == 0 ? stream : expandableStream;

                    switch (result.MessageType) {
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
            catch (Exception e) {
                ErrorOccurred?.Invoke(e);
            }

            if (wasSocketAlive && !IsSocketAlive()) {
                Console.WriteLine($"Disconnected {_url}");
                await Task.Delay(2000);
            }
        }
    }
}