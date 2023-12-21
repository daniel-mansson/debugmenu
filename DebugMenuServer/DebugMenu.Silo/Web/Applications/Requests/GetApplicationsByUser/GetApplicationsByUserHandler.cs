using DebugMenu.Silo.Web.Applications.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetApplicationsByUser; 

public class GetApplicationsByUserHandler : IRequestHandler<GetApplicationsByUserRequest, IReadOnlyList<ApplicationDto>> {
    private readonly IApplicationsRepository _applicationsRepository;

    public GetApplicationsByUserHandler(IApplicationsRepository applicationsRepository) {
        _applicationsRepository = applicationsRepository;
    }

    public async Task<IReadOnlyList<ApplicationDto>> Handle(GetApplicationsByUserRequest request, CancellationToken cancellationToken) {
        var applications =  await _applicationsRepository.GetByUserIdAsync(request.UserId);

        return applications
            .Select(app => new ApplicationDto() {
                Id = app.Id,
                Name = app.Name
            })
            .ToList();
    }
}