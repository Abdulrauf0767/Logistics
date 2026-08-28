using Logistics.Domain.Interfaces.JwtProvider;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Logistics.Infrastructure.Authentication.JwtProviders
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;
        public JwtProvider (IConfiguration configuration) {
            _configuration = configuration;
        }
        public string GenerateToken (int userId, string roleName , List<MappingPermissions> mappingPermissions)
        {
            var secretKey = _configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("Jwt secret is missing");
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expireString = _configuration["JwtSettings:ExpireInMinutes"] ?? throw new InvalidOperationException("ExpireInMinutes is missing in config");
            int expireInMinutes = int.Parse(expireString);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Role, roleName)
            };
            foreach (var permission in mappingPermissions)
            {
                if (!string.IsNullOrEmpty(permission.PermissionName))
                {
                    claims.Add(new Claim("permission", permission.PermissionName));
                }
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expireInMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
