using AutoMapper;
using DebugMenu.Silo.Web.Applications.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetApplication; 

public class GetApplicationHandler : IRequestHandler<GetApplicationRequest, ApplicationDto> {
    private readonly IMapper _mapper;
    private readonly IApplicationsRepository _applicationsRepository;

    public GetApplicationHandler(IMapper mapper, IApplicationsRepository applicationsRepository) {
        _mapper = mapper;
        _applicationsRepository = applicationsRepository;
    }

    public async Task<ApplicationDto> Handle(GetApplicationRequest request, CancellationToken cancellationToken) {
        var application = await _applicationsRepository.GetByIdAsync(request.Id);
        return _mapper.Map<ApplicationDto>(application);
    }
}