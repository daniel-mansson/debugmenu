using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetUsersInTeam;

public class GetUsersInTeamRequest : IRequest<IReadOnlyList<TeamUserDto>> {

}

public class GetAllTeamsHandler : IRequestHandler<GetUsersInTeamRequest, IReadOnlyList<TeamUserDto>> {
    public Task<IReadOnlyList<TeamUserDto>> Handle(GetUsersInTeamRequest request, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}
