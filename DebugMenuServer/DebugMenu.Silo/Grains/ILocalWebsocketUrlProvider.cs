namespace DebugMenu.Silo.Grains;

public interface ILocalWebsocketUrlProvider {
    string GetLocalUrl();
}

public class LocalWebsocketUrlProvider : ILocalWebsocketUrlProvider {
    public string GetLocalUrl() {
        return "wss://localhost:8082";
    }
}