using DebugMenu.Silo.Web.Applications.Persistence;
using DebugMenu.Silo.Web.RunningInstances.Grains;
using MediatR;

namespace DebugMenu.Silo.Web.RunningInstances.Requests.ValidateUserAccess;

public class ValidateUserAccessHandler : IRequestHandler<ValidateUserAccessRequest, bool> {
    private readonly IClusterClient _clusterClient;
    private readonly IApplicationsRepository _applicationsRepository;

    public ValidateUserAccessHandler(IClusterClient clusterClient, IApplicationsRepository applicationsRepository) {
        _clusterClient = clusterClient;
        _applicationsRepository = applicationsRepository;
    }

    public async Task<bool> Handle(ValidateUserAccessRequest request, CancellationToken cancellationToken) {
        var grain = _clusterClient.GetGrain<IRunningInstanceGrain>(request.InstanceId);

        var instanceDetails = await grain.GetDetails();

        var applicationEntity = await _applicationsRepository.GetByIdAsync(instanceDetails.ApplicationId);
        if (applicationEntity == null) {
            throw new Exception("Application not found");
        }

        var userFound = applicationEntity.Team.Users.Any(u => u.Id == request.UserId);
        return userFound;
    }
}
