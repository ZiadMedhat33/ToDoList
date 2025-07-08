using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Entites.Users;

namespace ToDoList.Services
{
    public class JwtService
    {
        private const string SecretKey = "THIS_IS_A_VERY_SECRET_KEY_FOR_JWT"; // to make it simple i just gonna leave it here and not care about it.
        private readonly SymmetricSecurityKey _key;

        public JwtService()
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "ToDoListAPI",
                audience: "ToDoListClient",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "ToDoListAPI",
                ValidateAudience = true,
                ValidAudience = "ToDoListClient",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true
            };
        }
    }
}
