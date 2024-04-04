namespace DebugMenu.Silo.Grains;

public interface ILocalWebsocketUrlProvider {
    string GetLocalUrl();
}

public class LocalWebsocketUrlProvider : ILocalWebsocketUrlProvider {
    public string GetLocalUrl() {
#if DEBUG
        return "wss://localhost:8082";
#else
        return "wss://debugmenu-5ttpcqzivq-lz.a.run.app";
#endif
    }
}
