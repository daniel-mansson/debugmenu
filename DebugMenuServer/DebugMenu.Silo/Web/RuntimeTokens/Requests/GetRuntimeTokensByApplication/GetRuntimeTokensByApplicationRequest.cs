using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.GetRuntimeTokensByApplication; 

public class GetRuntimeTokensByApplicationRequest : IRequest<IReadOnlyList<RuntimeTokenDto>> {
    public int ApplicationId { get; set; }
    public string Token { get; set; }
}