using AutoMapper;
using DebugMenu.Silo.Web.Teams.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetTeamsByUser;

public class GetTeamsByUserHandler : IRequestHandler<GetTeamsByUserRequest, IReadOnlyList<TeamDto>> {
    private readonly ITeamsRepository _teamsRepository;
    private readonly IMapper _mapper;

    public GetTeamsByUserHandler(ITeamsRepository teamsRepository, IMapper mapper) {
        _teamsRepository = teamsRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TeamDto>> Handle(GetTeamsByUserRequest request, CancellationToken cancellationToken) {
        //TODO: Validate current user
        var teams =  await _teamsRepository.GetByUserIdAsync(request.UserId);
        return _mapper.Map<IReadOnlyList<TeamDto>>(teams);
    }
}
