using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetTeamsByUser;


public class GetTeamsByUserRequest : IRequest<IReadOnlyList<TeamDto>> {
    public int UserId { get; set; }
}
