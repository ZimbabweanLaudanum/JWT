
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

class Program
{
    private const string SecretKey = "H2+FhC9q&bZ3RrE!dD8%Xy7!Lm2Pj5Wq"; 
    private static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

    static void Main(string[] args)
    {
        while (true) {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Создать JWT токен");
            Console.WriteLine("2. Проверить токен");
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                CreateToken();
                break;
            }
            else if (choice == "2")
            {
                ValidateToken();
                break;
            }
            else
            {
                Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                continue;
            }
        }
    }

    private static void CreateToken()
    {
        Console.Write("Введите имя пользователя: ");
        var username = Console.ReadLine();
        Console.Write("Введите пароль: ");
        var password = Console.ReadLine();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("custom_claim", "custom_value") 
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // Время жизни токена
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        Console.WriteLine("Сгенерированный JWT токен:");
        Console.WriteLine(tokenString);
    }

    private static void ValidateToken()
    {
        Console.Write("Введите JWT токен для проверки: ");
        var tokenToValidate = Console.ReadLine();

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(tokenToValidate, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SigningKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            Console.WriteLine("Токен действителен: true");
        }
        catch (Exception)
        {
            Console.WriteLine("Токен недействителен: false");
        }
    }
}