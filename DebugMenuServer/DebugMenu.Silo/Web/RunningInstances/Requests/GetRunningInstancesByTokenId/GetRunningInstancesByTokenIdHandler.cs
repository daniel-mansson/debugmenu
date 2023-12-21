using DebugMenu.Silo.Web.RunningInstances.Grains;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.GetRunningInstancesByTokenId; 

public class GetRunningInstancesByTokenHandler : IRequestHandler<GetRunningInstancesByTokenIdRequest, IReadOnlyList<RunningInstanceDto>> {
    private readonly IClusterClient _clusterClient;
    private readonly IRuntimeTokenRepository _runtimeTokenRepository;

    public GetRunningInstancesByTokenHandler(IClusterClient clusterClient, IRuntimeTokenRepository runtimeTokenRepository) {
        _clusterClient = clusterClient;
        _runtimeTokenRepository = runtimeTokenRepository;
    }

    public async Task<IReadOnlyList<RunningInstanceDto>> Handle(GetRunningInstancesByTokenIdRequest request, CancellationToken cancellationToken) {
        var tokenEntity = await _runtimeTokenRepository.GetByIdAsync(request.Id);
        if (tokenEntity == null) {
            return new List<RunningInstanceDto>();
        }
        
        var grain = _clusterClient.GetGrain<IRunningInstancesByTokenGrain>(tokenEntity.Token);
        return await grain.GetAllInstances();
    }
}