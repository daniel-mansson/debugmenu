using System.Net.WebSockets;

namespace DebugMenu.Silo.Grains;

public interface IWebsocketManager {
    WebSocket Get(string id);
    void Put(string id, WebSocket webSocket);
    void Remove(string id);

    WebSocketRoom GetRoom(string id);
}