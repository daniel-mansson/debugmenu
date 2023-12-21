using AutoMapper;
using DebugMenu.Silo.Web.Applications.Persistence;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence;
using DebugMenu.Silo.Web.RuntimeTokens.Persistence.EntityFramework;
using MediatR;

namespace DebugMenu.Silo.Web.RuntimeTokens.Requests.CreateRuntimeToken;

public class CreateRuntimeTokenHandler : IRequestHandler<CreateRuntimeTokenRequest, RuntimeTokenDto> {
    private readonly IRuntimeTokenRepository _runtimeTokenRepository;
    private readonly IApplicationsRepository _applicationsRepository;
    private readonly IMapper _mapper;
    private readonly Random _random = new();

    public CreateRuntimeTokenHandler(IRuntimeTokenRepository runtimeTokenRepository,
        IApplicationsRepository applicationsRepository,
        IMapper mapper) {
        _runtimeTokenRepository = runtimeTokenRepository;
        _applicationsRepository = applicationsRepository;
        _mapper = mapper;
    }

    public async Task<RuntimeTokenDto> Handle(CreateRuntimeTokenRequest request, CancellationToken cancellationToken) {
        var application = await _applicationsRepository.GetByIdAsync(request.ApplicationId);
        if (application == null) {
            throw new Exception("Not found");
        }

        var runtimeToken = _runtimeTokenRepository.Create(new RuntimeTokenEntity() {
            Name = request.InitialData.Name,
            Description = request.InitialData.Description,
            Application = application,
            Token = GenerateToken(24)
        });
        await _runtimeTokenRepository.SaveAsync();

        return _mapper.Map<RuntimeTokenDto>(runtimeToken);
    }

    private string GenerateToken(int length) {
        var bytes = new byte[length];
        _random.NextBytes(bytes);
        return Convert.ToHexString(bytes);
    }
}