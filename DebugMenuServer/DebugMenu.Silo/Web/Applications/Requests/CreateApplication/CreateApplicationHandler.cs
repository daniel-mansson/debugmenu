using DebugMenu.Silo.Web.Applications.Persistence;
using DebugMenu.Silo.Web.Applications.Persistence.EntityFramework;
using DebugMenu.Silo.Web.Users.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DebugMenu.Silo.Web.Applications.Requests.CreateApplication;

public class CreateApplicationHandler : IRequestHandler<CreateApplicationRequest, ApplicationDto> {
    private readonly IApplicationsRepository _applicationsRepository;
    private readonly IUserRepository _userRepository;

    public CreateApplicationHandler(IApplicationsRepository applicationsRepository, IUserRepository userRepository) {
        _applicationsRepository = applicationsRepository;
        _userRepository = userRepository;
    }

    public async Task<ApplicationDto> Handle(CreateApplicationRequest request, CancellationToken cancellationToken) {
        //TODO: Validate current user is set as owner OR is super admin

        var application = _applicationsRepository.Create(new ApplicationEntity() {
            Name = request.Item.Name
        });
        await _applicationsRepository.SaveAsync();

        if (request.OwnerUserId != null) {
            var owner = await _userRepository.GetByIdAsync(request.OwnerUserId);
            if (owner != null) {
                application.Users.Add(owner);
                application.ApplicationUsers.Add(new() {
                    ApplicationId = application.Id,
                    Application = application,
                    UserId = owner.Id,
                    User = owner,
                    Role = ApplicationMemberRole.Admin
                });
                await _applicationsRepository.SaveAsync();
            }
        }

        return new ApplicationDto() {
            Id = application.Id,
            Name = application.Name
        };
    }
}
