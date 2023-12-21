using DebugMenu.Silo.Web.Applications.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetAllApplications;

public class GetAllApplicationsHandler : IRequestHandler<GetAllApplicationsRequest, IReadOnlyList<ApplicationDto>> {
    private readonly IApplicationsRepository _applicationsRepository;

    public GetAllApplicationsHandler(IApplicationsRepository applicationsRepository) {
        _applicationsRepository = applicationsRepository;
    }

    public async Task<IReadOnlyList<ApplicationDto>> Handle(GetAllApplicationsRequest request, CancellationToken cancellationToken) {
        var applications =  await _applicationsRepository.GetAsync();

        return applications
            .Select(app => new ApplicationDto() {
                Id = app.Id,
                Name = app.Name
            })
            .ToList();
    }
}