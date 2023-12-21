using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.GetAllRuntimeTokens; 

public class GetAllRuntimeTokensRequest : IRequest<IReadOnlyList<RuntimeTokenDto>> {
}