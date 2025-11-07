using Auth_api.Models;

namespace Auth_api.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);
        Task<User?> GetByIdIncludeDeletedAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}