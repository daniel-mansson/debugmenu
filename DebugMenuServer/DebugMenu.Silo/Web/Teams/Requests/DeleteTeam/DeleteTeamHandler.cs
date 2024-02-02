using DebugMenu.Silo.Web.Teams.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.DeleteTeam;

public class DeleteTeamHandler : IRequestHandler<DeleteTeamRequest> {
    private readonly ITeamsRepository _teamsRepository;

    public DeleteTeamHandler(ITeamsRepository teamsRepository) {
        _teamsRepository = teamsRepository;
    }

    public async Task Handle(DeleteTeamRequest request, CancellationToken cancellationToken) {
        //TODO: Validate current user
        await _teamsRepository.DeleteAsync(request.Id);
        await _teamsRepository.SaveAsync();
    }
}
