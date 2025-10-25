using Auth_api.Models;
using Auth_api.UserDTOs;

namespace Auth_api.Services
{
    public interface IUserService
    {
        Task CreateAsync(User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<LoginResponseDto> LoginAsync(string username, string password);
        Task RestoreAsync(int id);
        Task<User?> GetByIdIncludeDeletedAsync(int id);
    }
}