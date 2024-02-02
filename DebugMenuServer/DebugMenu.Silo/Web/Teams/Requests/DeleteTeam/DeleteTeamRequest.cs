using DebugMenu.Silo.Web.Teams.Requests.CreateTeam;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.DeleteTeam;

public class DeleteTeamRequest : IRequest {
    public int Id { get; set; }
}
