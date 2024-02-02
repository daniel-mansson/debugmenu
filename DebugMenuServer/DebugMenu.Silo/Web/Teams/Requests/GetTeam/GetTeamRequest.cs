using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetTeam;


public class GetTeamRequest : IRequest<TeamDto> {

}

public class GetAllTeamsHandler : IRequestHandler<GetTeamRequest, TeamDto> {
    public Task<TeamDto> Handle(GetTeamRequest request, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}
