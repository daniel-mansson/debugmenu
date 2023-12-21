using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetUsersInApplication; 

public class GetUsersInApplicationRequest : IRequest<IReadOnlyList<ApplicationUserDto>> {
    public int ApplicationId { get; set; }
}