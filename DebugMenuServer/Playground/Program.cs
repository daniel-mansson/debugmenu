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
    audience:"https://debugmenu.io",
    issuer:"debugmenu.io"
);
var jwt = new JwtSecurityTokenHandler().WriteToken(token2);

string token =
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImRtIn0.eyJpZCI6ImRwNHBrM2wyYXJxZjI5NGZhdXhlb212OTQyd2t3OTIwbWVvM3Y4YmsiLCJ1c2VySWQiOiJiY2VpZ3Vqc3k4eXU0ZHIiLCJmcmVzaCI6ZmFsc2UsImV4cGlyZXNBdCI6IjIwMjQtMDUtMDRUMjA6MjA6MDcuNjg2WiIsImlhdCI6MTcxMjI2MjAxMCwiZXhwIjoxNzEyODY2ODEwLCJhdWQiOiJodHRwczovL2RlYnVnbWVudS5pbyIsImlzcyI6ImRlYnVnbWVudS5pbyIsInN1YiI6ImJjZWlndWpzeTh5dTRkciJ9.Og4LqrUhJc_agFEnpL39A0bBrv5qEO_MxRzvukEj51E";

//token = jwt;
try {
    var tokenHandler = new JwtSecurityTokenHandler();
    tokenHandler.ValidateToken(token, _parameters, out var validatedToken);
    Console.WriteLine(validatedToken);
}
catch(SecurityTokenInvalidAudienceException ex) {
    Console.WriteLine(ex.Message + " \nInvalid audience: " + ex.InvalidAudience + "\n\n" + token);
}
catch(SecurityTokenValidationException ex) {
    Console.WriteLine(ex.Message + " \n\n" + token);
}
