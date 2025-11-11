using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public interface IUserSyncRepository
    {
        Task<Usuario?> GetUsuarioByIdAsync(int userId);
        Task<Usuario> CreateUsuarioAsync(int userId, string username, string email);
        Task<bool> AsignarSucursalesDefaultAsync(int userId);
        Task<List<Sucursale>> GetSucursalesDefaultAsync();
    }
}