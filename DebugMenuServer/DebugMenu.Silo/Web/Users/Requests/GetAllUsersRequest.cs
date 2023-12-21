using AutoMapper;
using DebugMenu.Silo.Web.Users.Persistence;
using MediatR;

namespace DebugMenu.Silo.Web.Users.Requests;

public class GetAllUsersRequest : IRequest<IReadOnlyList<UserDto>> {
    
}

public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, IReadOnlyList<UserDto>> {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsersHandler(IUserRepository userRepository, IMapper mapper) {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<UserDto>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken) {
        var users = await _userRepository.GetAsync();

        return _mapper.Map<IReadOnlyList<UserDto>>(users);
    }
}