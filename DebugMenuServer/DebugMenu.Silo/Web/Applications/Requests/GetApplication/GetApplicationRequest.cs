using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.GetApplication;

public class GetApplicationRequest : IRequest<ApplicationDto> {
    public int Id { get; set; }
}