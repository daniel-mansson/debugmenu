using AutoMapper;
using DebugMenu.Silo.Web.Teams.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetTeam;

public class GetTeamHandler : IRequestHandler<GetTeamRequest, TeamDto> {
    private readonly ITeamsRepository _teamsRepository;
    private readonly IMapper _mapper;

    public GetTeamHandler(ITeamsRepository teamsRepository, IMapper mapper) {
        _teamsRepository = teamsRepository;
        _mapper = mapper;
    }

    public async Task<TeamDto> Handle(GetTeamRequest request, CancellationToken cancellationToken) {
        //TODO: validate current user is in team
        var team = await _teamsRepository.GetByIdAsync(request.Id);
        return _mapper.Map<TeamDto>(team);
    }
}
