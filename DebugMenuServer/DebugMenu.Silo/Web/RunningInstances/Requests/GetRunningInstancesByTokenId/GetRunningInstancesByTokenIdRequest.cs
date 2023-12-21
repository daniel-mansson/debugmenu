using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.GetRunningInstancesByTokenId;

public class GetRunningInstancesByTokenIdRequest : IRequest<IReadOnlyList<RunningInstanceDto>> {
    public int Id { get; set; }
}