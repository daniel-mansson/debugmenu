using DebugMenu.Silo.Web.RunningInstances.Grains;
using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.GetRunningInstancesByToken; 

public class GetRunningInstancesByTokenHandler : IRequestHandler<GetRunningInstancesByTokenRequest, IReadOnlyList<RunningInstanceDto>> {
    private readonly IClusterClient _clusterClient;

    public GetRunningInstancesByTokenHandler(IClusterClient clusterClient) {
        _clusterClient = clusterClient;
    }

    public async Task<IReadOnlyList<RunningInstanceDto>> Handle(GetRunningInstancesByTokenRequest request, CancellationToken cancellationToken) {
        var grain = _clusterClient.GetGrain<IRunningInstancesByTokenGrain>(request.Token);
        return await grain.GetAllInstances();
    }
}