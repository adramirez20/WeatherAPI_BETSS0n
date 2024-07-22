using Microsoft.EntityFrameworkCore;
using WeatherAPIIntegration.Domain.Entities;
using WeatherAPIIntegration.Infrastructure.Persistence;

namespace WeatherAPIIntegration.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }
    }
}
