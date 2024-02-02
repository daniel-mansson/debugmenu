using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetTeam;


public class GetTeamRequest : IRequest<TeamDto> {
    public int Id { get; set; }
}
