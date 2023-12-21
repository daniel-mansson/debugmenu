using System.Diagnostics;
using System.Net.WebSockets;
using DebugMenu.Silo.Abstractions;
using Orleans.Runtime;
using Orleans.Streams;

namespace DebugMenu.Silo.Grains;

public class DebugInstanceGrain : Grain, IDebugInstanceGrain {
    private readonly ILocalWebsocketUrlProvider _localWebsocketUrlProvider;
    private readonly IWebsocketManager _websocketManager;
    private WebSocketRoom _room;

    public DebugInstanceGrain(ILocalWebsocketUrlProvider localWebsocketUrlProvider, IWebsocketManager websocketManager) {
        _localWebsocketUrlProvider = localWebsocketUrlProvider;
        _websocketManager = websocketManager;
        _room = _websocketManager.GetRoom(this.GetPrimaryKeyString());
    }

    public Task<StartInstanceResponseDto> StartInstance(StartInstanceRequestDto request) {
        return Task.FromResult(new StartInstanceResponseDto(this.GetPrimaryKeyString(),
            _localWebsocketUrlProvider.GetLocalUrl() + "/ws/room/" + this.GetPrimaryKeyString()));
    }

    public Task<string> GetWebSocketUrl() {
        return Task.FromResult(_localWebsocketUrlProvider.GetLocalUrl() + "/ws/room/" + this.GetPrimaryKeyString());
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken) {
        RegisterTimer(OnTimerTick, this, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private Task OnTimerTick(object state) {
        if(_room.HasAnyActiveConnection()) {
            DelayDeactivation(TimeSpan.FromMinutes(2));
        }

        Console.WriteLine($"[Instance {this.GetPrimaryKeyString()}] HasInstance: {_room.Instance != null}, Num Controllers: {_room.Controllers.Count}");
        return Task.CompletedTask;
    }
}