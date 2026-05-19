using StudentPerformanceApi.Models;

namespace StudentPerformanceApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User> AddAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
}
