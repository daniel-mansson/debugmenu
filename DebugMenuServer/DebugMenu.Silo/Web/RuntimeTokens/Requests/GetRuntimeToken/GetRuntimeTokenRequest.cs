using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.GetRuntimeToken;

public class GetRuntimeTokenRequest : IRequest<RuntimeTokenDto> {
    public string Token { get; set; }
}