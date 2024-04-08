using System.Text.Json;
using DebugMenu.Silo.Web.Teams;
using DebugMenu.Silo.Web.Teams.Persistence;
using DebugMenu.Silo.Web.Teams.Persistence.EntityFramework;
using DebugMenu.Silo.Web.Teams.Requests.CreateTeam;
using MediatR;

namespace DebugMenu.Silo.Web.Startup.Requests.ProcessStartupEvents;

public class ProcessStartupEventsHandler : IRequestHandler<ProcessStartupEventsRequest, StartupEventsDto> {
    private readonly ITeamsRepository _teamsRepository;
    private readonly IMediator _mediator;

    public ProcessStartupEventsHandler(ITeamsRepository teamsRepository, IMediator mediator) {
        _teamsRepository = teamsRepository;
        _mediator = mediator;
    }

    public async Task<StartupEventsDto> Handle(ProcessStartupEventsRequest request, CancellationToken cancellationToken) {
        //TODO: Validate current user
        var result = new StartupEventsDto();

        var teamCreatedEvent = await CreatePersonalTeamIfNotExists(request.UserId);
        if(teamCreatedEvent != null) {
            result.Events.Add(teamCreatedEvent);
        }

        return result;
    }

    private async Task<StartupEventDto?> CreatePersonalTeamIfNotExists(string userId) {
        var teams = await _teamsRepository.GetByUserIdAsync(userId);
        if(teams.Count != 0) {
            return null;
        }

        var team = await _mediator.Send(new CreateTeamRequest() {
            Item = new TeamDto() {
                Name = "Personal",
                Type = TeamType.Personal,
            },
            OwnerUserId = userId
        });

        return new StartupEventDto() {
            Name = "PersonalTeamCreated",
            Details = JsonSerializer.Serialize(team),
            Message = "Personal team created"
        };
    }
}
