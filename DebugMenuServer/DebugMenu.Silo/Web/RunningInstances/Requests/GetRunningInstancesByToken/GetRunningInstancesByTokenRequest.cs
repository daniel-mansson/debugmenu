using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.GetRunningInstancesByToken;

public class GetRunningInstancesByTokenRequest : IRequest<IReadOnlyList<RunningInstanceDto>> {
    public string Token { get; set; } = string.Empty;
}