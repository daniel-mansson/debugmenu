using AutoMapper;
using DebugMenu.Silo.Web.Applications.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetApplicationsByTeam;

public class GetApplicationsByTeamHandler : IRequestHandler<GetApplicationsByTeamRequest, IReadOnlyList<ApplicationDto>> {
    private readonly IApplicationsRepository _applicationsRepository;
    private readonly IMapper _mapper;

    public GetApplicationsByTeamHandler(IApplicationsRepository applicationsRepository, IMapper mapper) {
        _applicationsRepository = applicationsRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ApplicationDto>> Handle(GetApplicationsByTeamRequest request, CancellationToken cancellationToken) {
        var applications =  await _applicationsRepository.GetByTeamIdAsync(request.TeamId);
        return _mapper.Map<IReadOnlyList<ApplicationDto>>(applications);
    }
}
