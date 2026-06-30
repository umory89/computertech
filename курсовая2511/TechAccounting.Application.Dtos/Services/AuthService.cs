
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechAccounting.Data;
using курсовая2511.Models;

namespace курсовая2511.TechAccounting.Application.Dtos.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

      
        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);

         
            if (user == null || user.PasswordHash != password)
                return null;

            return user;
        }


        public async Task<(bool IsSuccess, string Message)> RegisterAsync(string username, string email, string password, string role = "Employee")
        {
      
            if (await _context.User.AnyAsync(u => u.Username == username))
                return (false, "Пользователь с таким логином уже существует!");

           
            if (await _context.User.AnyAsync(u => u.Email == email))
                return (false, "Эта электронная почта уже зарегистрирована!");

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = password, 
                Email = email,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return (true, "Регистрация успешно завершена!");
        }
    }
}
