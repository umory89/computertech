using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechAccounting.Data;

namespace курсовая2511.TechAccounting.Application.Dtos.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context) => _context = context;

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || user.PasswordHash != password) return null;

            return "mocked_jwt_token_for_" + user.Role;
        }
    }
}
