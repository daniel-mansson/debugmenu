using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.GetRunningInstance;

public class GetRunningInstanceRequest : IRequest<RunningInstanceDto> {
    public string Id { get; set; } = string.Empty;
}