using Pharm_api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Services.Interfaces
{
    public interface IArticuloService
    {
        Task<IEnumerable<ArticuloDto>> GetAllArticulosAsync();
        Task<ArticuloDto?> GetArticuloByIdAsync(int id);
        Task<IEnumerable<ArticuloDto>> GetBySucursalAsync(int codSucursal);
    }
}
