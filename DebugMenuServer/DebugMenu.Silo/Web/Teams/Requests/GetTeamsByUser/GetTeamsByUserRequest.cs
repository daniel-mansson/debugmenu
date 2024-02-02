using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetTeamsByUser;


public class GetTeamsByUserRequest : IRequest<IReadOnlyList<TeamDto>> {

}

public class GetAllTeamsHandler : IRequestHandler<GetTeamsByUserRequest, IReadOnlyList<TeamDto>> {
    public Task<IReadOnlyList<TeamDto>> Handle(GetTeamsByUserRequest request, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}
