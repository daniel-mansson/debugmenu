using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.CreateRuntimeToken;

public class CreateRuntimeTokenRequest : IRequest<RuntimeTokenDto> {
    public int ApplicationId { get; set; }
    public RuntimeTokenMutableDto InitialData { get; set; } = null!;
}