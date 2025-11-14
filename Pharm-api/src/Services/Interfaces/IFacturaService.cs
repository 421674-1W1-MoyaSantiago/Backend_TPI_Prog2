using Pharm_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pharm_api.DTOs;

namespace Pharm_api.Services
{
    public interface IFacturaService
    {
        Task<FacturaVentaDto?> GetFacturaConDetallesAsync(int codFacturaVenta);
        Task<IEnumerable<FacturaVentaDto>> GetFacturasByUsuarioAsync(int usuarioId);
        Task<bool> CreateFacturaAsync(CreateFacturaVentaDto createDto, int usuarioId);
        Task<bool> EditFacturaAsync(EditFacturaVentaDto createDto, int codFacturaVenta, int usuarioId);
        Task<bool> DeleteFacturaAsync(int codFacturaVenta, int usuarioId);
        Task<List<FormaPagoDto>> GetFormasPagoAsync();
    }
}