using DebugMenu.Silo.Web.RuntimeTokens.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.DeleteRuntimeToken; 

public class DeleteRuntimeTokenHandler : IRequestHandler<DeleteRuntimeTokenRequest> {
    private readonly IRuntimeTokenRepository _runtimeTokenRepository;

    public DeleteRuntimeTokenHandler(IRuntimeTokenRepository runtimeTokenRepository) {
        _runtimeTokenRepository = runtimeTokenRepository;
    }

    public async Task Handle(DeleteRuntimeTokenRequest request, CancellationToken cancellationToken) {
        await _runtimeTokenRepository.DeleteAsync(request.Id);
        await _runtimeTokenRepository.SaveAsync();
    }
}