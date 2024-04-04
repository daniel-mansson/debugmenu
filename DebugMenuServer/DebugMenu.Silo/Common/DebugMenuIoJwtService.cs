using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DebugMenu.Silo.Common;

public class DebugMenuIoJwtService : IJwtService {
    private readonly TokenValidationParameters _parameters;
    private const string SecretKey = "ed5b824bd6f6a2db591c0273d9fb176decae1c5f6b86e6ab03df741815827e90";

    public DebugMenuIoJwtService() {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        key.KeyId = "dm";
        _parameters = new() {
            ValidateIssuer = true,
            ValidIssuer = "debugmenu.io",
            ValidateAudience = true,
            ValidAudience = "https://debugmenu.io",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            IssuerSigningKeys = new[] { key },
            ValidateLifetime = true
        };
    }

    public JwtSecurityToken? Decode(string token) {
        try {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, _parameters, out SecurityToken validatedToken);
            return (JwtSecurityToken)validatedToken;
        }
        catch(SecurityTokenInvalidAudienceException ex) {
            Console.WriteLine(ex.Message + " \nInvalid audience: " + ex.InvalidAudience + "\n\n" + token);
        }
        catch(SecurityTokenValidationException ex) {
            Console.WriteLine(ex.Message + " \n\n" + token);
        }

        return null;
    }
}
