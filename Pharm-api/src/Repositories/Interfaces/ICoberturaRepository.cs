using Pharm_api.Models;
using Pharm_api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Repositories.Interfaces
{
    public interface ICoberturaRepository
    {
    Task<IEnumerable<CoberturaDto>> GetCoberturasBySucursalAsync(int codSucursal);
    Task<IEnumerable<CoberturaDto>> GetCoberturasByClienteAsync(int idCliente);
    }
}
