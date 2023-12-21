using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.DeleteRuntimeToken;

public class DeleteRuntimeTokenRequest : IRequest {
    public int Id { get; set; }
}