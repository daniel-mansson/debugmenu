using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.CreateApplication;

public class CreateApplicationRequest : IRequest<ApplicationDto> {
    public int OwnerTeamId { get; set; }
    public ApplicationDto Item { get; set; } = null!;
}
