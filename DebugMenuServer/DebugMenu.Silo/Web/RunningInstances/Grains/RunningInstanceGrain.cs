using DebugMenu.Silo.Grains;

namespace DebugMenu.Silo.Web.RunningInstances.Grains;

public class RunningInstanceGrain : Grain, IRunningInstanceGrain {
    private readonly ILocalWebsocketUrlProvider _localWebsocketUrlProvider;
    private readonly WebSocketRoom _room;
    private string? _token;
    private int _applicationId;
    private readonly RunningInstanceMetadata _metadata = new();

    public RunningInstanceGrain(ILocalWebsocketUrlProvider localWebsocketUrlProvider,
        IWebsocketManager websocketManager) {
        _localWebsocketUrlProvider = localWebsocketUrlProvider;
        _room = websocketManager.GetRoom(this.GetPrimaryKeyString());
        _room.ApiUpdated += OnApiUpdated;
    }

    private void OnApiUpdated(string api) {
        _metadata.Api = api;
    }

    public async Task<RunningInstanceDto> StartInstance(StartInstanceRequestDto request) {
        if (_metadata.IsInitialized) {
            if (request.Token != _token) {
                throw new Exception("Expected token to be same");
            }

            Console.WriteLine("Reinitializing instance");
        }
        else {
            _metadata.IsInitialized = true;
            _applicationId = request.ApplicationId;            
            _token = request.Token;
            _metadata.Metadata = request.Metadata;
            RegisterTimer(OnTimerTick, this, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            await GrainFactory.GetGrain<IRunningInstancesByTokenGrain>(_token)
                .Register(this);
        }

        _metadata.Metadata = request.Metadata;
        return BuildDetailsDto();
    }

    public Task<bool> IsValid(string token) {
        return Task.FromResult(_metadata.IsInitialized && _token == token);
    }

    public Task<RunningInstanceMetadata> GetMetadata() {
        return Task.FromResult(_metadata);
    }

    public Task<RunningInstanceDto> GetDetails() {
        if (!_metadata.IsInitialized) {
            throw new Exception("Not Found");
        }

        return Task.FromResult(BuildDetailsDto());
    }

    private RunningInstanceDto BuildDetailsDto() {
        return new RunningInstanceDto() {
            Id = this.GetPrimaryKeyString(),
            ConnectedViewers = _room.Controllers.Count,
            DeviceId = _metadata.Metadata.GetValueOrDefault("device_id", string.Empty),
            WebsocketUrl = _localWebsocketUrlProvider.GetLocalUrl() + "/ws/room/" + this.GetPrimaryKeyString(),
            HasConnectedInstance = _room.Instance != null,
            ApplicationId = _applicationId
        };
    }

    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken) {
        if (_metadata.IsInitialized) {
            await GrainFactory.GetGrain<IRunningInstancesByTokenGrain>(_token)
                .Unregister(this);
        }
    }

    private Task OnTimerTick(object state) {
        if (_room.HasAnyActiveConnection()) {
            DelayDeactivation(TimeSpan.FromMinutes(2));
        }

        Console.WriteLine(
            $"[Instance {this.GetPrimaryKeyString()}] HasInstance: {_room.Instance != null}, Num Controllers: {_room.Controllers.Count}");
        return Task.CompletedTask;
    }
}