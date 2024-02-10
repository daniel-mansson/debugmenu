using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetApplicationsByTeam;

public class GetApplicationsByTeamRequest : IRequest<IReadOnlyList<ApplicationDto>> {
    public int TeamId { get; set; }
}
