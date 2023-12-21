using AutoMapper;
using DebugMenu.Silo.Web.Applications.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.DeleteApplication;

public class DeleteApplicationHandler : IRequestHandler<DeleteApplicationRequest> {
    private readonly IMapper _mapper;
    private readonly IApplicationsRepository _applicationsRepository;

    public DeleteApplicationHandler(IMapper mapper, IApplicationsRepository applicationsRepository) {
        _mapper = mapper;
        _applicationsRepository = applicationsRepository;
    }

    public async Task Handle(DeleteApplicationRequest request, CancellationToken cancellationToken) {
        await _applicationsRepository.DeleteAsync(request.Id);
        await _applicationsRepository.SaveAsync();
    }
}