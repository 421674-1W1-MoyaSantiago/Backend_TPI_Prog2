using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public interface ISucursalRepository
    {
        Task<IEnumerable<Sucursale>> GetAllSucursalesAsync();
    }
}
