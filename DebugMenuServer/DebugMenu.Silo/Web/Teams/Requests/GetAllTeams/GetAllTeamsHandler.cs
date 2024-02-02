using AutoMapper;
using DebugMenu.Silo.Web.Teams.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.GetAllTeams;

public class GetAllTeamsHandler : IRequestHandler<GetAllTeamsRequest, IReadOnlyList<TeamDto>> {
    private readonly ITeamsRepository _teamsRepository;
    private readonly IMapper _mapper;

    public GetAllTeamsHandler(ITeamsRepository teamsRepository, IMapper mapper) {
        _teamsRepository = teamsRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TeamDto>> Handle(GetAllTeamsRequest request, CancellationToken cancellationToken) {
        //TODO: validate super user
        var teams =  await _teamsRepository.GetAsync();
        return _mapper.Map<IReadOnlyList<TeamDto>>(teams);
    }
}
