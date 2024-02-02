using AutoMapper;
using DebugMenu.Silo.Web.Teams.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetUsersInTeam;

public class GetUsersInTeamHandler : IRequestHandler<GetUsersInTeamRequest, IReadOnlyList<TeamUserDto>> {
    private readonly ITeamsRepository _teamsRepository;
    private readonly IMapper _mapper;

    public GetUsersInTeamHandler(ITeamsRepository teamsRepository, IMapper mapper) {
        _teamsRepository = teamsRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TeamUserDto>> Handle(GetUsersInTeamRequest request, CancellationToken cancellationToken) {
        var users = await _teamsRepository.GetUsersInTeamAsync(request.TeamId);
        return _mapper.Map<IReadOnlyList<TeamUserDto>>(users);
    }
}
