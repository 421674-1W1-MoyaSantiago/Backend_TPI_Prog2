using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            var detalles = await _context.DetallesFacturaVentasMedicamento
                .Where(d => d.codFacturaVenta == codFacturaVenta)
                .Include(d => d.Medicamento)
                    .ThenInclude(m => m.CodTipoMedicamentoNavigation)
                .Include(d => d.Medicamento)
                    .ThenInclude(m => m.CodTipoPresentacionNavigation)
                .Include(d => d.Medicamento)
                    .ThenInclude(m => m.CodLaboratorioNavigation)
                .Include(d => d.Medicamento)
                    .ThenInclude(m => m.CodLoteMedicamentoNavigation)
                .Include(d => d.Medicamento)
                    .ThenInclude(m => m.CodUnidadMedidaNavigation)
                .Include(d => d.Cobertura)
                    .ThenInclude(c => c.CodObraSocialNavigation)
                .ToListAsync();

            return detalles.Select(d => new DetalleMedicamentoDto
            {
                CodDetFacVentaM = d.cod_DetFacVentaM,
                Cantidad = d.cantidad,
                PrecioUnitario = d.precioUnitario,
                CodCobertura = d.codCobertura,
                NombreCobertura = d.Cobertura != null && d.Cobertura.CodObraSocialNavigation != null ? d.Cobertura.CodObraSocialNavigation.RazonSocial : null,
                CodMedicamento = d.codMedicamento,
                NombreMedicamento = d.Medicamento != null ? d.Medicamento.Descripcion : string.Empty,
                Laboratorio = d.Medicamento != null && d.Medicamento.CodLaboratorioNavigation != null ? d.Medicamento.CodLaboratorioNavigation.Descripcion : null,
                Lote = d.Medicamento != null && d.Medicamento.CodLoteMedicamentoNavigation != null ? $"Lote {d.Medicamento.CodLoteMedicamentoNavigation.CodLoteMedicamento} - Vence: {d.Medicamento.CodLoteMedicamentoNavigation.FechaVencimiento:dd/MM/yyyy}" : null,
                Tipo = d.Medicamento != null && d.Medicamento.CodTipoMedicamentoNavigation != null ? d.Medicamento.CodTipoMedicamentoNavigation.Descripcion : null,
                Presentacion = d.Medicamento != null && d.Medicamento.CodTipoPresentacionNavigation != null ? d.Medicamento.CodTipoPresentacionNavigation.Descripcion : null,
                UnidadMedida = d.Medicamento != null && d.Medicamento.CodUnidadMedidaNavigation != null ? d.Medicamento.CodUnidadMedidaNavigation.UnidadMedida : null,
                Concentracion = null // Si tienes campo de concentración, aquí lo puedes mapear
            }).ToList();
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

            // Obtener detalles de medicamentos (incluye entidades completas)
            var detallesMedicamentoEntities = await _context.DetallesFacturaVentasMedicamento
                .Where(d => d.codFacturaVenta == codFacturaVenta)
                .Include(d => d.Medicamento)
                    .ThenInclude(m => m.CodTipoPresentacionNavigation)
                .Include(d => d.Cobertura)
                    .ThenInclude(c => c.CodObraSocialNavigation)
                .ToListAsync();

            var detallesMedicamento = detallesMedicamentoEntities.Select(d => new DetalleMedicamentoFacturaDto
            {
                CodigoDetalle = d.cod_DetFacVentaM,
                Cantidad = d.cantidad,
                PrecioUnitario = d.precioUnitario,
                CodMedicamento = d.codMedicamento,
                NombreMedicamento = d.Medicamento != null ? d.Medicamento.Descripcion : string.Empty,
                Concentracion = null, // Agregar cuando esté en el modelo
                Presentacion = d.Medicamento != null && d.Medicamento.CodTipoPresentacionNavigation != null ? d.Medicamento.CodTipoPresentacionNavigation.Descripcion : null,
                CodCobertura = d.codCobertura,
                NombreCobertura = d.Cobertura != null && d.Cobertura.CodObraSocialNavigation != null ? d.Cobertura.CodObraSocialNavigation.RazonSocial : null
            }).ToList();

            // Obtener detalles de artículos (incluye entidades completas)
            var detallesArticuloEntities = await _context.DetallesFacturaVentasArticulo
                .Where(d => d.codFacturaVenta == codFacturaVenta)
                .Include(d => d.Articulo)
                .ToListAsync();

            var detallesArticulo = detallesArticuloEntities.Select(d => new DetalleArticuloFacturaDto
            {
                CodigoDetalle = d.cod_DetFacVentaA,
                Cantidad = d.cantidad,
                PrecioUnitario = d.precioUnitario,
                CodArticulo = d.codArticulo,
                NombreArticulo = d.Articulo != null ? d.Articulo.Descripcion : string.Empty,
                Marca = null // Agregar cuando esté en el modelo
            }).ToList();

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

        public async Task<bool> CreateFacturaForUsuarioAsync
            (FacturasVentum factura, int usuarioId,
            IEnumerable<DetallesFacturaVentasArticulo>? detalleArticulos = null,
            IEnumerable<DetallesFacturaVentasMedicamento>? detalleMedicamentos = null)
        {
            // Insertar factura solo si el usuario tiene acceso a la sucursal
            var tieneAcceso = await _context.Grupsucursales
                .AnyAsync(gs => gs.CodUsuario == usuarioId && gs.CodSucursal == factura.CodSucursal && gs.Activo);
            if (!tieneAcceso)
                return false;

            // Guardar la nueva factura creada y obtener el id generado
            EntityEntry<FacturasVentum> nuevaFactura = await _context.FacturasVenta.AddAsync(factura);
            if (await _context.SaveChangesAsync() == 0) { return false; }
            int nuevaFacturaId = nuevaFactura.Entity.CodFacturaVenta;

            decimal totalFactura = 0; // Acumulador para calcular el total de la factura

            if (detalleArticulos != null)
            {
                foreach (var detalle in detalleArticulos)
                {
                    // iterar cada detalle, obtener el precio de la db, asignar el id de factura y guardar
                    var precioArt = await _context.Articulos
                        .AsNoTracking()
                        .Where(a => a.CodArticulo == detalle.codArticulo)
                        .Select(a => (decimal?)a.PrecioUnitario) // nullable para detectar no-encontrado
                        .FirstOrDefaultAsync();

                    if (precioArt == null) continue; // si no existe el artículo, saltar

                    detalle.codFacturaVenta = nuevaFacturaId;
                    detalle.precioUnitario = precioArt.Value;
                    totalFactura += detalle.precioUnitario * detalle.cantidad; 
                    await _context.DetallesFacturaVentasArticulo.AddAsync(detalle);
                }
            }

            if (detalleMedicamentos != null)
            {
                foreach (var detalle in detalleMedicamentos)
                {
                    // iterar cada detalle, obtener el precio de la db, asignar el id de factura y guardar
                    var precioMed = await _context.Medicamentos
                        .AsNoTracking()
                        .Where(a => a.CodMedicamento == detalle.codMedicamento)
                        .Select(a => (decimal?)a.PrecioUnitario) // nullable para detectar no-encontrado
                        .FirstOrDefaultAsync();

                    if (precioMed == null) continue; // si no existe el medicamento, saltar
                    detalle.codFacturaVenta = nuevaFacturaId;
                    detalle.precioUnitario = precioMed.Value;
                    totalFactura += detalle.precioUnitario * detalle.cantidad;
                    await _context.DetallesFacturaVentasMedicamento.AddAsync(detalle);
                }
            }

            // Actualizar el total de la factura
            factura.Total = totalFactura;
            _context.FacturasVenta.Update(factura);

            if (await _context.SaveChangesAsync() == 0) 
            { return false; } else { return true; }
        }
    }
}
