using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace DebugMenu.Silo;

public interface IWebSocketRoom {
}


public class WebSocketRoom : IWebSocketRoom {
    public string Id { get; }

    public WebSocket? Instance { get; private set; }
    public List<WebSocket> Controllers { get; } = new();

    public event Action<string> ApiUpdated;
    
    private object _lock = new();
    
    public WebSocketRoom(string id) {
        Id = id;
    }

    public async Task BroadcastToControllersAsync(byte[] buffer, int length, WebSocketMessageType type, bool endOfMessage, CancellationToken cancellationToken) {
        var memory = new ReadOnlyMemory<byte>(buffer, 0, length);

        List<WebSocket> controllers;
        lock (_lock) {
            controllers = Controllers.ToList();
        }

        if (IsInternalMessage(buffer, length, type, endOfMessage)) {
            HandleInternalMessage(buffer, length);
        }

        Console.WriteLine($"[Room {Id}] Broadcasting {length} bytes to {controllers.Count} controllers.");

        var tasks = controllers.Select(async socket => {
            if (!socket.CloseStatus.HasValue) {
                await socket.SendAsync(memory, type, endOfMessage, cancellationToken);
            }
        }).ToList();
        
        await Task.WhenAll(tasks);
    }

    private void HandleInternalMessage(byte[] buffer, int length) {
        var stream = new MemoryStream(buffer);
        var reader = new BinaryReader(stream);
        var channelLength = reader.ReadByte();
        var channelBytes = reader.ReadBytes(channelLength);
        var channel = Encoding.UTF8.GetString(channelBytes);

        if (!channel.StartsWith("__internal/")) {
            return;
        }

        if (channel == "__internal/api") {
            var textLength = length - channelLength - 1;
            var textBytes = reader.ReadBytes(textLength);
            var text = Encoding.UTF8.GetString(textBytes);
            ApiUpdated?.Invoke(text);
        }
    }

    private bool IsInternalMessage(byte[] buffer, int length, WebSocketMessageType type, bool endOfMessage) {
        return type == WebSocketMessageType.Binary 
               && endOfMessage 
               && length >= 3 
               && buffer[1] == '_' 
               && buffer[2] == '_';
    }

    public async Task SendToInstance(byte[] buffer, int length, WebSocketMessageType type, bool endOfMessage, CancellationToken cancellationToken) {
        if (Instance == null) {
            Console.WriteLine($"[Room {Id}] Trying to send {length} bytes to instance, but instance is not available");
            return;
        }

        Console.WriteLine($"[Room {Id}] Sending {length} bytes to instance.");

        var memory = new ReadOnlyMemory<byte>(buffer, 0, length);
        await Instance.SendAsync(memory, type, endOfMessage, cancellationToken);
    }

    public void AddController(WebSocket webSocket) {
        lock (_lock) {
            Controllers.Add(webSocket);
        }
        Console.WriteLine($"[Room {Id}] Controller added. {webSocket}");
    }
    public void RemoveController(WebSocket webSocket) {
        bool wasRemoved;
        lock (_lock) {
            wasRemoved = Controllers.Remove(webSocket);
        }
        Console.WriteLine($"[Room {Id}] Controller {(wasRemoved ? "removed" : "not found while removing")}.  {webSocket}");
    }

    public void SetInstance(WebSocket webSocket) {
        if (Instance != null) {
            Console.WriteLine($"[Room {Id}] Instance already set. Unexpected.");
            Instance
                .CloseAsync(WebSocketCloseStatus.ProtocolError, "Closed due to another instance being connected. Only one can be active at the same time.", CancellationToken.None)
                .Ignore();
        }

        Instance = webSocket;
        Console.WriteLine($"[Room {Id}] New instance registered. {webSocket}");
    }

    public void RemoveInstance(WebSocket webSocket) {
        if (Instance != webSocket) {
            Console.WriteLine($"[Room {Id}] Trying to remove instance which does not match socket. Unexpected.");
        }
        else {
            Instance = null;
            Console.WriteLine($"[Room {Id}] New instance registered. {webSocket}");

        }
    }

    public bool HasAnyActiveConnection() {
        return Instance != null || Controllers.Count > 0;
    }
}