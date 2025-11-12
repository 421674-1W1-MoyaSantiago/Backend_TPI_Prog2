using Pharm_api.Models;
using Pharm_api.DTOs;
using Pharm_api.Repositories.Interfaces;
using Pharm_api.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Services.Implementations
{
    public class CoberturaService : ICoberturaService
    {
        private readonly ICoberturaRepository _coberturaRepository;
        public CoberturaService(ICoberturaRepository coberturaRepository)
        {
            _coberturaRepository = coberturaRepository;
        }

        public async Task<IEnumerable<CoberturaDto>> GetCoberturasBySucursalAsync(int codSucursal)
        {
            return await _coberturaRepository.GetCoberturasBySucursalAsync(codSucursal);
        }

        public async Task<IEnumerable<CoberturaDto>> GetCoberturasByClienteAsync(int idCliente)
        {
            return await _coberturaRepository.GetCoberturasByClienteAsync(idCliente);
        }
    }
}
