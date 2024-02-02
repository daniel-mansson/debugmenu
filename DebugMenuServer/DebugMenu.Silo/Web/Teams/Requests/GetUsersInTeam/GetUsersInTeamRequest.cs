using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetUsersInTeam;

public class GetUsersInTeamRequest : IRequest<IReadOnlyList<TeamUserDto>> {
    public int TeamId { get; set; }
}
