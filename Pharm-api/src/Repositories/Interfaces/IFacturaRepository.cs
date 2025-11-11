using Pharm_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pharm_api.DTOs;

namespace Pharm_api.Repositories
{
    public interface IFacturaRepository
    {
        Task<List<DetalleMedicamentoDto>> GetDetallesMedicamentoAsync(int codFacturaVenta);
        Task<List<DetalleArticuloDto>> GetDetallesArticuloAsync(int codFacturaVenta);
        Task<List<DetalleFacturaBaseDto>> GetDetallesUnificadosAsync(int codFacturaVenta);
        Task<FacturaVentaDto?> GetFacturaConDetallesAsync(int codFacturaVenta);
        Task<IEnumerable<FacturasVentum>> GetFacturasByUsuarioAsync(int usuarioId);
        Task<bool> CreateFacturaForUsuarioAsync(
            FacturasVentum factura, int usuarioId,
            IEnumerable<DetallesFacturaVentasArticulo>? detalleArticulos = null,
            IEnumerable<DetallesFacturaVentasMedicamento>? detalleMedicamentos = null
        );
        Task<IEnumerable<FormasPago>> GetFormasPagoAsync();
    }
}