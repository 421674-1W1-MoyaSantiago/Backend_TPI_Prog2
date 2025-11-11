using Pharm_api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Repositories.Interfaces
{
    public interface IArticuloRepository
    {
        Task<IEnumerable<ArticuloDto>> GetAllAsync();
    Task<ArticuloDto?> GetByIdAsync(int id);
    Task<IEnumerable<ArticuloDto>> GetBySucursalAsync(int codSucursal);
    }
}
