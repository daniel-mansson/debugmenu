using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DebugMenu.Silo.Common;

public class DebugMenuIoJwtService :IJwtService {
    private readonly TokenValidationParameters _parameters = new() {
        ValidateIssuer = true,
        ValidIssuer = "debugmenu.io",
        ValidateAudience = true,
        ValidAudience = "http://debugmenu.io",
        ValidateIssuerSigningKey = true,
        IssuerSigningKeys = new []{new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))},
        ValidateLifetime = true
    };
    private const string SecretKey = "ed5b824bd6f6a2db592c0273d9fb176decae1c5f6b86e6ab03df741815827e90";

    public JwtSecurityToken? Decode(string token) {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, _parameters, out SecurityToken validatedToken);
            return (JwtSecurityToken)validatedToken;
        } catch (SecurityTokenValidationException ex) {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}