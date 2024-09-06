using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TecnoMundo.Identity.Model;

namespace TecnoMundo.Identity.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(
                _configuration.GetSection("AuthenticationSettings:Secret").Value
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration.GetSection("AuthenticationSettings:ProvaiderToken").Value,
                Audience = _configuration.GetSection("AuthenticationSettings:AudienceToken").Value,
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, $"{user.UserName} {user.LastName}"),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(
                    Convert.ToDouble(
                        _configuration.GetSection("AuthenticationSettings:ExpiressHours").Value
                    )
                ),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
