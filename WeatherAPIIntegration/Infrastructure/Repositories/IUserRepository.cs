using WeatherAPIIntegration.Domain.Entities;

namespace WeatherAPIIntegration.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetByUsernameAsync(string username);
        public Task<bool> UsernameExistsAsync(string username);
    
        Task AddAsync(User user);
    }
}
