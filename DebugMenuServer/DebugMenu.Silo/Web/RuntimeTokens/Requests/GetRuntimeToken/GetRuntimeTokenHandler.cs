using AutoMapper;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.GetRuntimeToken; 

public class GetRuntimeTokenHandler : IRequestHandler<GetRuntimeTokenRequest, RuntimeTokenDto> {
    private readonly IRuntimeTokenRepository _runtimeTokenRepository;
    private readonly IMapper _mapper;

    public GetRuntimeTokenHandler(IRuntimeTokenRepository runtimeTokenRepository, IMapper mapper) {
        _runtimeTokenRepository = runtimeTokenRepository;
        _mapper = mapper;
    }

    public async Task<RuntimeTokenDto> Handle(GetRuntimeTokenRequest request, CancellationToken cancellationToken) {
        var runtimeTokenEntity = await _runtimeTokenRepository.GetByTokenAsync(request.Token);

        return _mapper.Map<RuntimeTokenDto>(runtimeTokenEntity);
    }
}