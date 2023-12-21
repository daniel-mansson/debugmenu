using AutoMapper;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence;
using DebugMenu.Silo.Web.RuntimeTokens.Requests.GetRuntimeTokensByApplication;
using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.GetAllRuntimeTokens; 

public class GetAllRuntimeTokensHandler : IRequestHandler<GetRuntimeTokensByApplicationRequest,
    IReadOnlyList<RuntimeTokenDto>> {
    private readonly IRuntimeTokenRepository _runtimeTokenRepository;
    private readonly IMapper _mapper;

    public GetAllRuntimeTokensHandler(IRuntimeTokenRepository runtimeTokenRepository, IMapper mapper) {
        _runtimeTokenRepository = runtimeTokenRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<RuntimeTokenDto>> Handle(GetRuntimeTokensByApplicationRequest byApplicationRequest,
        CancellationToken cancellationToken) {
        var tokens = await _runtimeTokenRepository.GetAsync();

        return _mapper.Map<IReadOnlyList<RuntimeTokenDto>>(tokens);
    }
}