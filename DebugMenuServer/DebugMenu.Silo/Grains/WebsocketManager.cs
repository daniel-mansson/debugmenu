using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace DebugMenu.Silo.Grains;

public class WebsocketManager : IWebsocketManager {
    private ConcurrentDictionary<string, WebSocket> _webSockets = new();
    private ConcurrentDictionary<string, WebSocketRoom> _webSocketRooms = new();
    
    public WebSocket Get(string id) {
        return _webSockets[id];
    }

    public void Put(string id, WebSocket webSocket) {
        _webSockets[id] = webSocket;
    }

    public void Remove(string id) {
        _webSockets.Remove(id, out _);
    }

    public WebSocketRoom GetRoom(string id) {
        return _webSocketRooms.GetOrAdd(id, (key) => new WebSocketRoom(key));
    }
}