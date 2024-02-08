using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.CreateApplication;

public class CreateApplicationRequest : IRequest<ApplicationDto> {
    public string? OwnerUserId { get; set; }
    public ApplicationDto Item { get; set; } = null!;
}
