using DebugMenu.Silo.Web.Teams.Requests.CreateTeam;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetAllTeams;

public class GetAllTeamsRequest : IRequest<IReadOnlyList<TeamDto>> {

}
