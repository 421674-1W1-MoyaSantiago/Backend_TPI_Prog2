using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Services
{
    public interface ISucursalService
    {
        Task<IEnumerable<SucursalDto>> GetAllSucursalesAsync();
    }
}
