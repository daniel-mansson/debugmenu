using MediatR;

namespace DebugMenu.Silo.Web.Applications.Requests.DeleteApplication;

public class DeleteApplicationRequest : IRequest {
    public int Id { get; set; }
}