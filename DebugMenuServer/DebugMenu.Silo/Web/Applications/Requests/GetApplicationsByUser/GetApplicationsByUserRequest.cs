using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetApplicationsByUser;

public class GetApplicationsByUserRequest : IRequest<IReadOnlyList<ApplicationDto>> {
    public int UserId { get; set; }
}