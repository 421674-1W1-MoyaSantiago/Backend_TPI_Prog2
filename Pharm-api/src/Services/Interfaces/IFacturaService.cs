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
        Task<bool> CreateFacturaForUsuarioAsync(CreateFacturaVentaDto createDto, int usuarioId);
        Task<bool> EditFacturaForUsuarioAsync(EditFacturaVentaDto createDto, int codFacturaVenta, int usuarioId);
        Task<List<DetalleMedicamentoDto>> GetDetallesMedicamentoAsync(int facturaId);
        Task<List<DetalleFacturaBaseDto>> GetDetallesUnificadosAsync(int facturaId);
        Task<List<FormaPagoDto>> GetFormasPagoAsync();
    }
}