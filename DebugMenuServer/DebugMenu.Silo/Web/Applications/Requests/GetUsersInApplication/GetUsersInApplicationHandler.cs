using AutoMapper;
using DebugMenu.Silo.Web.Applications.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetUsersInApplication;

public class GetUsersInApplicationHandler 
    : IRequestHandler<GetUsersInApplicationRequest, IReadOnlyList<ApplicationUserDto>> {
    private readonly IApplicationsRepository _applicationsRepository;
    private readonly IMapper _mapper;

    public GetUsersInApplicationHandler(IApplicationsRepository applicationsRepository, IMapper mapper) {
        _applicationsRepository = applicationsRepository;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<ApplicationUserDto>> Handle(GetUsersInApplicationRequest request, CancellationToken cancellationToken) {
        var users = await _applicationsRepository.GetUsersInApplicationAsync(request.ApplicationId);

        return _mapper.Map<IReadOnlyList<ApplicationUserDto>>(users);
    }
}