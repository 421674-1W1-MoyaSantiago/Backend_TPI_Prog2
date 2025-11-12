using Pharm_api.Models;
using Pharm_api.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Services.Interfaces
{
    public interface ICoberturaService
    {
    Task<IEnumerable<CoberturaDto>> GetCoberturasBySucursalAsync(int codSucursal);
    Task<IEnumerable<CoberturaDto>> GetCoberturasByClienteAsync(int idCliente);
    }
}
