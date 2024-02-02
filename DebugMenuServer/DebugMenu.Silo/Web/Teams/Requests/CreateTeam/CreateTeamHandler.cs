using AutoMapper;
using DebugMenu.Silo.Web.Teams.Persistence;
using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;
using DebugMenu.Silo.Web.Users.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Teams.Requests.CreateTeam;

public class CreateTeamHandler : IRequestHandler<CreateTeamRequest, TeamDto> {
    private readonly IUserRepository _userRepository;
    private readonly ITeamsRepository _teamsRepository;
    private readonly IMapper _mapper;

    public CreateTeamHandler(IUserRepository userRepository, ITeamsRepository teamsRepository, IMapper mapper) {
        _userRepository = userRepository;
        _teamsRepository = teamsRepository;
        _mapper = mapper;
    }

    public async Task<TeamDto> Handle(CreateTeamRequest request, CancellationToken cancellationToken) {
        //TODO: Validate current user is set as owner OR is super admin

        var team = _teamsRepository.Create(new TeamEntity() {
            Name = request.Item.Name
        });
        await _teamsRepository.SaveAsync();

        if (request.OwnerUserId.HasValue) {
            var owner = await _userRepository.GetByIdAsync(request.OwnerUserId.GetValueOrDefault());
            if (owner != null) {
                team.Users.Add(owner);
                team.TeamUsers.Add(new() {
                    TeamId = team.Id,
                    Team = team,
                    UserId = owner.Id,
                    User = owner,
                    Role = TeamMemberRole.Owner
                });
                await _teamsRepository.SaveAsync();
            }
        }

        return _mapper.Map<TeamDto>(team);
    }
}
