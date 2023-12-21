using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetAllApplications;

public class GetAllApplicationsRequest : IRequest<IReadOnlyList<ApplicationDto>> {
}