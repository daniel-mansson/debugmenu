using DebugMenu.Silo.Web.RunningInstances.Grains;
using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.GetRunningInstance; 

public class GetRunningInstanceHandler : IRequestHandler<GetRunningInstanceRequest, RunningInstanceDto> {
    private readonly IClusterClient _clusterClient;

    public GetRunningInstanceHandler(IClusterClient clusterClient) {
        _clusterClient = clusterClient;
    }

    public async Task<RunningInstanceDto> Handle(GetRunningInstanceRequest request, CancellationToken cancellationToken) {
        var grain = _clusterClient.GetGrain<IRunningInstanceGrain>(request.Id);
        return await grain.GetDetails();
    }
}