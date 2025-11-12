using Pharm_api.DTOs;
using Pharm_api.Repositories.Interfaces;
using Pharm_api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Services.Implementations
{
    public class ArticuloService : IArticuloService
    {
        private readonly IArticuloRepository _articuloRepository;
        public ArticuloService(IArticuloRepository articuloRepository)
        {
            _articuloRepository = articuloRepository;
        }

        public async Task<IEnumerable<ArticuloDto>> GetAllArticulosAsync()
        {
            return await _articuloRepository.GetAllAsync();
        }

        public async Task<ArticuloDto?> GetArticuloByIdAsync(int id)
        {
            return await _articuloRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ArticuloDto>> GetBySucursalAsync(int codSucursal)
        {
            return await _articuloRepository.GetBySucursalAsync(codSucursal);
        }
    }
}
