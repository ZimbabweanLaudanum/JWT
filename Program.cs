using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using static JWT.TokenService;

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        var tokenService = new JWT.TokenService();
        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Создать JWT токен");
            Console.WriteLine("2. Проверить токен");
            var choice = Console.ReadLine();
            if (choice == "1")
            {

                Console.Write("Введите имя пользователя: ");
                var username = Console.ReadLine();
                Console.Write("Введите пароль: ");
                var password = Console.ReadLine();

                // Генерация токена
                var token = tokenService.GenerateToken(username, password);
                Console.WriteLine("Сгенерированный токен: " + token);
                Console.WriteLine("Нажмите Enter для окончания");
                Console.ReadLine();
                continue;
            }
            else if (choice == "2")
            {
                Console.Write("Введите токен: ");
                var token = Console.ReadLine();
                // Проверка токена
                var validationResult = tokenService.ValidateToken(token);
                if (validationResult.IsValid)
                {
                    Console.WriteLine("Токен действителен!");
                    Console.WriteLine($"Имя пользователя: {validationResult.Username}");
                    Console.WriteLine($"Пароль: {validationResult.Password}");
                }
                else
                {
                    Console.WriteLine("Токен недействителен.");
                }
                Console.WriteLine("Нажмите Enter для окончания");
                Console.ReadLine();
                continue;
            }
            else
            {
                Console.WriteLine("Введено неверное значение. Повторите попытку.");
                continue;
            }
        }
    }
}