using DebugMenu.Silo.Web.Applications.Persistence;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;
using DebugMenu.Silo.Web.Teams.Persistence;
using DebugMenu.Silo.Web.Users.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DebugMenu.Silo.Web.Applications.Requests.CreateApplication;

public class CreateApplicationHandler : IRequestHandler<CreateApplicationRequest, ApplicationDto> {
    private readonly IApplicationsRepository _applicationsRepository;
    private readonly ITeamsRepository _teamsRepository;

    public CreateApplicationHandler(IApplicationsRepository applicationsRepository, ITeamsRepository teamsRepository) {
        _applicationsRepository = applicationsRepository;
        _teamsRepository = teamsRepository;
    }

    public async Task<ApplicationDto> Handle(CreateApplicationRequest request, CancellationToken cancellationToken) {
        //TODO: Validate current user is set as owner OR is super admin
        var ownerTeam = await _teamsRepository.GetByIdAsync(request.OwnerTeamId);
        if(ownerTeam == null) {
            throw new Exception("Team not found");
        }

        var application = _applicationsRepository.Create(new ApplicationEntity() {
            Name = request.Item.Name,
            TeamId = request.OwnerTeamId,
            Team = ownerTeam
        });

        await _applicationsRepository.SaveAsync();

        return new ApplicationDto() {
            Id = application.Id,
            Name = application.Name
        };
    }
}
