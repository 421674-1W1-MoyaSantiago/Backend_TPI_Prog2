using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly PharmDbContext _context;

        public FacturaRepository(PharmDbContext context)
        {
            _context = context;
        }
       

        public async Task<List<DetalleMedicamentoDto>> GetDetallesMedicamentoAsync(int codFacturaVenta)
        {
            return await _context.DetallesFacturaVentasMedicamento
                .Where(d => d.codFacturaVenta == codFacturaVenta)
                .Include(d => d.Medicamento)
                    .ThenInclude(m => m.CodTipoMedicamentoNavigation)
                .Include(d => d.Medicamento)
                    .ThenInclude(m => m.CodTipoPresentacionNavigation)
                .Include(d => d.Cobertura)
                    .ThenInclude(c => c.CodObraSocialNavigation)
                .Select(d => new DetalleMedicamentoDto
                {
                    CodDetFacVentaM = d.cod_DetFacVentaM,
                    Cantidad = d.cantidad,
                    PrecioUnitario = d.precioUnitario,
                    CodCobertura = d.codCobertura,
                    NombreCobertura = d.Cobertura != null ? d.Cobertura.CodObraSocialNavigation.RazonSocial : null,
                    CodMedicamento = d.codMedicamento,
                    NombreMedicamento = d.Medicamento.Descripcion,
                    Concentracion = null, // Por ahora null hasta agregar al modelo
                    Presentacion = d.Medicamento.CodTipoPresentacionNavigation != null ? 
                        d.Medicamento.CodTipoPresentacionNavigation.Descripcion : null
                }).ToListAsync();
        }

        public async Task<List<DetalleArticuloDto>> GetDetallesArticuloAsync(int codFacturaVenta)
        {
            return await _context.DetallesFacturaVentasArticulo
                .Where(d => d.codFacturaVenta == codFacturaVenta)
                .Include(d => d.Articulo)
                .Select(d => new DetalleArticuloDto
                {
                    CodDetFacVentaA = d.cod_DetFacVentaA,
                    Cantidad = d.cantidad,
                    PrecioUnitario = d.precioUnitario,
                    CodArticulo = d.codArticulo,
                    NombreArticulo = d.Articulo.Descripcion,
                    Marca = null // Por ahora null hasta agregar al modelo
                }).ToListAsync();
        }
      
        public async Task<List<DetalleFacturaBaseDto>> GetDetallesUnificadosAsync(int codFacturaVenta)
        {
            var detalles = new List<DetalleFacturaBaseDto>();

            // Obtener detalles de medicamentos
            var detallesMedicamento = await _context.DetallesFacturaVentasMedicamento
                .Where(d => d.codFacturaVenta == codFacturaVenta)
                .Include(d => d.Medicamento)
                    .ThenInclude(m => m.CodTipoPresentacionNavigation)
                .Include(d => d.Cobertura)
                    .ThenInclude(c => c.CodObraSocialNavigation)
                .Select(d => new DetalleMedicamentoFacturaDto
                {
                    CodigoDetalle = d.cod_DetFacVentaM,
                    Cantidad = d.cantidad,
                    PrecioUnitario = d.precioUnitario,
                    CodMedicamento = d.codMedicamento,
                    NombreMedicamento = d.Medicamento.Descripcion,
                    Concentracion = null, // Agregar cuando esté en el modelo
                    Presentacion = d.Medicamento.CodTipoPresentacionNavigation != null ? 
                        d.Medicamento.CodTipoPresentacionNavigation.Descripcion : null,
                    CodCobertura = d.codCobertura,
                    NombreCobertura = d.Cobertura != null && d.Cobertura.CodObraSocialNavigation != null ? 
                        d.Cobertura.CodObraSocialNavigation.RazonSocial : null
                }).ToListAsync();

            // Obtener detalles de artículos
            var detallesArticulo = await _context.DetallesFacturaVentasArticulo
                .Where(d => d.codFacturaVenta == codFacturaVenta)
                .Include(d => d.Articulo)
                .Select(d => new DetalleArticuloFacturaDto
                {
                    CodigoDetalle = d.cod_DetFacVentaA,
                    Cantidad = d.cantidad,
                    PrecioUnitario = d.precioUnitario,
                    CodArticulo = d.codArticulo,
                    NombreArticulo = d.Articulo.Descripcion,
                    Marca = null // Agregar cuando esté en el modelo
                }).ToListAsync();

            detalles.AddRange(detallesMedicamento);
            detalles.AddRange(detallesArticulo);

            return detalles.OrderBy(d => d.CodigoDetalle).ToList();
        }

        public async Task<FacturaVentaDto?> GetFacturaConDetallesAsync(int codFacturaVenta)
        {
            var factura = await _context.FacturasVenta
                .Include(f => f.CodEmpleadoNavigation)
                .Include(f => f.CodClienteNavigation)
                .Include(f => f.CodSucursalNavigation)
                .Include(f => f.CodFormaPagoNavigation)
                .FirstOrDefaultAsync(f => f.CodFacturaVenta == codFacturaVenta);

            if (factura == null) return null;

            // Usar el nuevo método unificado
            var detallesUnificados = await GetDetallesUnificadosAsync(codFacturaVenta);

            return new FacturaVentaDto
            {
                CodFacturaVenta = factura.CodFacturaVenta,
                Fecha = factura.Fecha,
                CodEmpleado = factura.CodEmpleado,
                NombreEmpleado = $"{factura.CodEmpleadoNavigation.NomEmpleado} {factura.CodEmpleadoNavigation.ApeEmpleado}",
                CodCliente = factura.CodCliente,
                NombreCliente = $"{factura.CodClienteNavigation.NomCliente} {factura.CodClienteNavigation.ApeCliente}",
                CodSucursal = factura.CodSucursal,
                NombreSucursal = factura.CodSucursalNavigation.NomSucursal,
                CodFormaPago = factura.CodFormaPago,
                MetodoPago = factura.CodFormaPagoNavigation.Metodo,
                Total = factura.Total ?? 0,
                Detalles = detallesUnificados
            };
        }
        public async Task<IEnumerable<FacturasVentum>> GetFacturasByUsuarioAsync(int usuarioId)
        {
            var sucursalIds = await _context.Grupsucursales
                .Where(gs => gs.CodUsuario == usuarioId && gs.Activo)
                .Select(gs => gs.CodSucursal)
                .ToListAsync();

            return await _context.FacturasVenta
                .Where(f => sucursalIds.Contains(f.CodSucursal))
                .Include(f => f.CodEmpleadoNavigation)
                .Include(f => f.CodClienteNavigation)
                .Include(f => f.CodSucursalNavigation)
                .Include(f => f.CodFormaPagoNavigation)
                .ToListAsync();
        }

        // Insertar factura solo si el usuario tiene acceso a la sucursal
        public async Task<bool> CreateFacturaForUsuarioAsync(FacturasVentum factura, int usuarioId)
        {
            var tieneAcceso = await _context.Grupsucursales
                .AnyAsync(gs => gs.CodUsuario == usuarioId && gs.CodSucursal == factura.CodSucursal && gs.Activo);
            if (!tieneAcceso)
                return false;

            _context.FacturasVenta.Add(factura);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
