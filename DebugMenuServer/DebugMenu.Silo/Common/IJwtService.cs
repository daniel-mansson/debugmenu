using System.IdentityModel.Tokens.Jwt;

namespace DebugMenu.Silo.Common; 

public interface IJwtService {
    JwtSecurityToken? Decode(string token);
}