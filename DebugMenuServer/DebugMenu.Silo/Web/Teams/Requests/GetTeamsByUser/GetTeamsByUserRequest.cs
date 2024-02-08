using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetTeamsByUser;


public class GetTeamsByUserRequest : IRequest<IReadOnlyList<TeamDto>> {
    public string UserId { get; set; }
}
