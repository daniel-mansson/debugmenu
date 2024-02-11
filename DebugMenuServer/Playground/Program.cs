using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

const string SecretKey = "ed5b824bd6f6a2db591c0273d9fb176decae1c5f6b86e6ab03df741815827e90";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
key.KeyId = "dm";
var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

TokenValidationParameters _parameters = new() {
    ValidateIssuer = true,
    ValidIssuer = "debugmenu.io",
    ValidateAudience = true,
    ValidAudience = "http://debugmenu.io",
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = key,
    ValidateLifetime = true,
};
var token2 = new JwtSecurityToken(
    claims: new []{new Claim(ClaimTypes.Sid, "asdf")},
    expires: DateTime.UtcNow.AddDays(1),
    signingCredentials: cred,
    audience:"http://debugmenu.io",
    issuer:"debugmenu.io"
);
var jwt = new JwtSecurityTokenHandler().WriteToken(token2);

string token =
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImRtIn0.eyJkYXRhIjoiaGVqIiwiaWF0IjoxNzA3NjkxNTQxLCJleHAiOjE3MDgyOTYzNDEsImF1ZCI6Imh0dHA6Ly9kZWJ1Z21lbnUuaW8iLCJpc3MiOiJkZWJ1Z21lbnUuaW8iLCJzdWIiOiJ1c2VyIn0.FDxcFzVpGWqNpY2Si7wnqWVzbnCiIpVJXDNr2FuReXk";

//token = jwt;
try {
    var tokenHandler = new JwtSecurityTokenHandler();
    tokenHandler.ValidateToken(token, _parameters, out var validatedToken);
    Console.WriteLine(validatedToken);
}
catch(SecurityTokenValidationException ex) {
    Console.WriteLine(ex.Message);
}
