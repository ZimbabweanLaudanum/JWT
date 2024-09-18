using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT
{
    public class TokenService
    {
        private readonly string _secretKey = "H2+FhC9q&bZ3RrE!dD8%Xy7!Lm2Pj5Wq"; // Замените на ваш секретный ключ
        private readonly int _expiryDuration = 30; // Время существования токена в минутах

        public string GenerateToken(string username, string password)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("password", password) // Добавляем пароль (хотя это не рекомендуется)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expiryDuration),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (bool IsValid, string Username, string Password) ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
                var password = jwtToken.Claims.First(claim => claim.Type == "password").Value;

                return (true, username, password);
            }
            catch
            {
                return (false, null, null);
            }
        }
    }
}