using AutoMapper;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.GetRuntimeTokensByApplication;

public class GetRuntimeTokensByApplicationHandler : IRequestHandler<GetRuntimeTokensByApplicationRequest,
    IReadOnlyList<RuntimeTokenDto>> {
    private readonly IRuntimeTokenRepository _runtimeTokenRepository;
    private readonly IMapper _mapper;

    public GetRuntimeTokensByApplicationHandler(IRuntimeTokenRepository runtimeTokenRepository, IMapper mapper) {
        _runtimeTokenRepository = runtimeTokenRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<RuntimeTokenDto>> Handle(GetRuntimeTokensByApplicationRequest byApplicationRequest,
        CancellationToken cancellationToken) {
        var tokens = await _runtimeTokenRepository.GetByApplicationAsync(byApplicationRequest.ApplicationId);

        return _mapper.Map<IReadOnlyList<RuntimeTokenDto>>(tokens);
    }
}