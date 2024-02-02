using AutoMapper;
using DebugMenu.Silo.Web.Applications.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.DeleteApplication;

public class DeleteApplicationHandler : IRequestHandler<DeleteApplicationRequest> {
    private readonly IApplicationsRepository _applicationsRepository;

    public DeleteApplicationHandler(IApplicationsRepository applicationsRepository) {
        _applicationsRepository = applicationsRepository;
    }

    public async Task Handle(DeleteApplicationRequest request, CancellationToken cancellationToken) {
        //TODO: Validate current user
        await _applicationsRepository.DeleteAsync(request.Id);
        await _applicationsRepository.SaveAsync();
    }
}
