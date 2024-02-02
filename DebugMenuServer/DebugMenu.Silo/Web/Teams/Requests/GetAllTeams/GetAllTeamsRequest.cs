using DebugMenu.Silo.Web.Teams.Requests.CreateTeam;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetAllTeams;

public class GetAllTeamsRequest : IRequest<IReadOnlyList<TeamDto>> {

}

public class GetAllTeamsHandler : IRequestHandler<GetAllTeamsRequest, IReadOnlyList<TeamDto>> {
    public Task<IReadOnlyList<TeamDto>> Handle(GetAllTeamsRequest request, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}
