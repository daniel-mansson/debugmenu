using DebugMenu.Silo.Web.Applications;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.CreateTeam;

public class CreateTeamRequest : IRequest<TeamDto> {
    public int? OwnerUserId { get; set; }
    public TeamDto Item { get; set; } = null!;
}
