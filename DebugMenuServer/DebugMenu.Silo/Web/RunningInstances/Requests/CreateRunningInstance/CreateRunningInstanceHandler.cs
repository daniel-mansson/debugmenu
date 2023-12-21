using DebugMenu.Silo.Web.Applications.Persistence;
using DebugMenu.Silo.Web.RunningInstances.Grains;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.CreateRunningInstance;

public class CreateRunningInstanceHandler : IRequestHandler<CreateRunningInstanceRequest, RunningInstanceDto> {
    private readonly IClusterClient _clusterClient;
    private readonly IRuntimeTokenRepository _runtimeTokenRepository;

    public CreateRunningInstanceHandler(IClusterClient clusterClient, IRuntimeTokenRepository runtimeTokenRepository) {
        _clusterClient = clusterClient;
        _runtimeTokenRepository = runtimeTokenRepository;
    }

    public async Task<RunningInstanceDto> Handle(CreateRunningInstanceRequest request, CancellationToken cancellationToken) {
        var guid = Guid.NewGuid();
        var grain = _clusterClient.GetGrain<IRunningInstanceGrain>(guid.ToString());

        var tokenEntity = await _runtimeTokenRepository.GetByTokenAsync(request.Token);
        if (tokenEntity == null) {
            throw new Exception("No matching token entry");
        }
        
        var result = await grain.StartInstance(
            new StartInstanceRequestDto(
                request.Token, 
                request.Metadata,
                tokenEntity.Application.Id));

        return result;
    }
}