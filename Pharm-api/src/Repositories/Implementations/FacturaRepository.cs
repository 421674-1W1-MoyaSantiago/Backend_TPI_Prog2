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

        public async Task<List<DetalleFacturaBaseDto>> GetDetallesUnificadosAsync(int codFacturaVenta)
        {
            var detalles = new List<DetalleFacturaBaseDto>();

            // Obtener detalles de medicamentos (incluye entidades completas)
            var detallesMedicamentoEntities = await _context.DetallesFacturaVentasMedicamento
                .Where(d => d.codFacturaVenta == codFacturaVenta && !d.Anulada)
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
                NombreCobertura = d.Cobertura != null && d.Cobertura.CodObraSocialNavigation != null ? d.Cobertura.CodObraSocialNavigation.RazonSocial : null,
                Codigo = d.codMedicamento
            }).ToList();

            // Obtener detalles de artículos (incluye entidades completas)
            var detallesArticuloEntities = await _context.DetallesFacturaVentasArticulo
                .Where(d => d.codFacturaVenta == codFacturaVenta && !d.Anulada)
                .Include(d => d.Articulo)
                .ToListAsync();

            var detallesArticulo = detallesArticuloEntities.Select(d => new DetalleArticuloFacturaDto
            {
                CodigoDetalle = d.cod_DetFacVentaA,
                Cantidad = d.cantidad,
                PrecioUnitario = d.precioUnitario,
                CodArticulo = d.codArticulo,
                NombreArticulo = d.Articulo != null ? d.Articulo.Descripcion : string.Empty,
                Codigo = d.codArticulo
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
                .FirstOrDefaultAsync(f => f.CodFacturaVenta == codFacturaVenta && !f.Anulada);

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
                .Where(f => sucursalIds.Contains(f.CodSucursal) && !f.Anulada)
                .Include(f => f.CodEmpleadoNavigation)
                .Include(f => f.CodClienteNavigation)
                .Include(f => f.CodSucursalNavigation)
                .Include(f => f.CodFormaPagoNavigation)
                .ToListAsync();
        }

        public async Task<bool> CreateFacturaAsync
            (FacturasVentum factura, int usuarioId,
            IEnumerable<DetallesFacturaVentasArticulo>? detalleArticulos = null,
            IEnumerable<DetallesFacturaVentasMedicamento>? detalleMedicamentos = null)
        {
            // Insertar factura solo si el usuario tiene acceso a la sucursal
            var tieneAcceso = await _context.Grupsucursales
                .AnyAsync(gs => gs.CodUsuario == usuarioId && gs.CodSucursal == factura.CodSucursal && gs.Activo);
            if (!tieneAcceso)
                return false;

            // Iniciar una transacción
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try 
            {
                // Guardar la nueva factura creada y obtener el id generado
                EntityEntry<FacturasVentum> nuevaFactura = await _context.FacturasVenta.AddAsync(factura);
                if (await _context.SaveChangesAsync() == 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
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
                        if (precioArt == null)
                        {
                            throw new ArgumentException($"No existe el artículo con código '{detalle.codArticulo}'.");
                        }

                        var stockArt = await _context.StockArticulos
                            .Where(s => s.CodArticulo == detalle.codArticulo && s.CodSucursal == factura.CodSucursal)
                            .FirstOrDefaultAsync();
                        if (stockArt == null)
                        {
                            throw new ArgumentException($"No existe stock registrado para el artículo con código '{detalle.codArticulo}' en la sucursal N°{factura.CodSucursal}.");
                        }

                        if (stockArt.Cantidad < detalle.cantidad)
                        {
                            throw new InvalidOperationException($"No hay suficiente stock para el artículo con código '{detalle.codArticulo}'. Stock disponible: {stockArt.Cantidad}, cantidad solicitada: {detalle.cantidad}.");
                        }

                        stockArt.Cantidad -= detalle.cantidad;
                        _context.StockArticulos.Update(stockArt);

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

                        if (precioMed == null)
                        {
                            throw new ArgumentException($"No existe el medicamento con código '{detalle.codMedicamento}'.");
                        }

                        var stockMed = await _context.StockMedicamentos
                            .Where(s => s.CodMedicamento == detalle.codMedicamento && s.CodSucursal == factura.CodSucursal)
                            .FirstOrDefaultAsync();
                        if (stockMed == null)
                        {
                            throw new ArgumentException($"No existe stock registrado para el medicamento con código '{detalle.codMedicamento}'.");
                        }

                        if (stockMed.Cantidad < detalle.cantidad)
                        {
                            throw new ArgumentException($"No hay suficiente stock para el medicamento con código '{detalle.codMedicamento}'. Stock disponible: {stockMed.Cantidad}, cantidad solicitada: {detalle.cantidad}.");
                        }

                        stockMed.Cantidad -= detalle.cantidad;
                        _context.StockMedicamentos.Update(stockMed);

                        detalle.codFacturaVenta = nuevaFacturaId;
                        detalle.precioUnitario = precioMed.Value;
                        totalFactura += detalle.precioUnitario * detalle.cantidad;
                        await _context.DetallesFacturaVentasMedicamento.AddAsync(detalle);
                    }
                }

                // Actualizar el total de la factura
                factura.Total = totalFactura;
                _context.FacturasVenta.Update(factura);

                // Guardar todos los cambios
                await _context.SaveChangesAsync();

                // Confirmar la transacción si todo salió bien
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                // Si hay cualquier error, revertir todos los cambios
                await transaction.RollbackAsync();
                throw; 
            }
        }

        public async Task<bool> EditFacturaAsync(FacturasVentum factura, int usuarioId, 
            IEnumerable<DetallesFacturaVentasArticulo>? detalleArticulos = null, 
            IEnumerable<DetallesFacturaVentasMedicamento>? detalleMedicamentos = null)
        {
            // Verificar que la factura existe
            var facturaExistente = await _context.FacturasVenta.FirstOrDefaultAsync(f => f.CodFacturaVenta == factura.CodFacturaVenta);
            if (facturaExistente == null)
                throw new ArgumentException($"No existe una factura con Codigo '{factura.CodFacturaVenta}'.");

            // Verificar que el usuario tiene acceso a la sucursal
            var tieneAcceso = await _context.Grupsucursales
                .AnyAsync(gs => gs.CodUsuario == usuarioId && gs.CodSucursal == facturaExistente.CodSucursal && gs.Activo);
            
            if (!tieneAcceso)
                return false;
                        

            decimal totalFactura = 0; // Acumulador para calcular el total de la factura

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Obtener detalles existentes de artículos
                var detallesArticulosExistentes = await _context.DetallesFacturaVentasArticulo
                .Where(d => d.codFacturaVenta == factura.CodFacturaVenta)
                .ToListAsync();

                if (detalleArticulos != null)
                {
                    var listaDetalleArticulos = detalleArticulos.ToList();

                    // Procesar cada detalle que viene en la request
                    foreach (var detalleNuevo in listaDetalleArticulos)
                    {
                        // Obtener el precio actual de la base de datos
                        var precioArt = await _context.Articulos
                            .AsNoTracking()
                            .Where(a => a.CodArticulo == detalleNuevo.codArticulo)
                            .Select(a => (decimal?)a.PrecioUnitario)
                            .FirstOrDefaultAsync();

                        if (precioArt == null)
                        {
                            throw new ArgumentException($"No existe el artículo con código '{detalleNuevo.codArticulo}'.");
                        }

                        var stockArt = await _context.StockArticulos
                            .Where(s => s.CodArticulo == detalleNuevo.codArticulo && s.CodSucursal == facturaExistente.CodSucursal)
                            .FirstOrDefaultAsync();

                        if (stockArt == null)
                        {
                            throw new ArgumentException($"No existe stock registrado para el artículo con código '{detalleNuevo.codArticulo}' en la sucursal N°{facturaExistente.CodSucursal}.");
                        }

                        var detalleExistente = detallesArticulosExistentes.FirstOrDefault(d => d.codArticulo == detalleNuevo.codArticulo);

                        if (detalleExistente != null)
                        {
                            // ACTUALIZAR detalle existente
                            int diferenciaCantidad = detalleNuevo.cantidad - detalleExistente.cantidad;

                            // Validar stock disponible considerando la cantidad que ya tenía
                            if (diferenciaCantidad > 0 && stockArt.Cantidad < diferenciaCantidad)
                            {
                                throw new InvalidOperationException(
                                    $"No hay suficiente stock para aumentar la cantidad del artículo '{detalleNuevo.codArticulo}'. " +
                                    $"Stock disponible: {stockArt.Cantidad}, cantidad adicional requerida: {diferenciaCantidad}.");
                            }

                            stockArt.Cantidad -= diferenciaCantidad;
                            _context.StockArticulos.Update(stockArt);

                            detalleExistente.cantidad = detalleNuevo.cantidad;
                            detalleExistente.precioUnitario = precioArt.Value;
                            _context.DetallesFacturaVentasArticulo.Update(detalleExistente);
                            totalFactura += detalleExistente.precioUnitario * detalleExistente.cantidad;
                        }
                        else
                        {
                            // CREAR nuevo detalle
                            if (stockArt.Cantidad < detalleNuevo.cantidad)
                            {
                                throw new InvalidOperationException(
                                    $"No hay suficiente stock para el artículo con código '{detalleNuevo.codArticulo}'. " +
                                    $"Stock disponible: {stockArt.Cantidad}, cantidad solicitada: {detalleNuevo.cantidad}.");
                            }

                            stockArt.Cantidad -= detalleNuevo.cantidad;
                            _context.StockArticulos.Update(stockArt);

                            detalleNuevo.codFacturaVenta = factura.CodFacturaVenta;
                            detalleNuevo.precioUnitario = precioArt.Value;
                            totalFactura += detalleNuevo.precioUnitario * detalleNuevo.cantidad;
                            await _context.DetallesFacturaVentasArticulo.AddAsync(detalleNuevo);
                        }
                    }

                    // ANULAR detalles que ya no vienen en la request y DEVOLVER STOCK
                    var codigosArticulosNuevos = listaDetalleArticulos.Select(d => d.codArticulo).ToList();
                    var detallesAEliminar = detallesArticulosExistentes
                        .Where(d => !codigosArticulosNuevos.Contains(d.codArticulo))
                        .ToList();

                    foreach (var detalle in detallesAEliminar)
                    {
                        var stockArt = await _context.StockArticulos
                            .Where(s => s.CodArticulo == detalle.codArticulo && s.CodSucursal == facturaExistente.CodSucursal)
                            .FirstOrDefaultAsync();

                        if (stockArt != null)
                        {
                            stockArt.Cantidad += detalle.cantidad;
                            _context.StockArticulos.Update(stockArt);
                        }

                        detalle.Anulada = true;
                        _context.DetallesFacturaVentasArticulo.Update(detalle);
                    }
                }
                else
                {
                    // Si no vienen detalles de artículos, anular TODOS y devolver stock
                    foreach (var detalle in detallesArticulosExistentes)
                    {
                        var stockArt = await _context.StockArticulos
                            .Where(s => s.CodArticulo == detalle.codArticulo && s.CodSucursal == facturaExistente.CodSucursal)
                            .FirstOrDefaultAsync();

                        if (stockArt != null)
                        {
                            stockArt.Cantidad += detalle.cantidad;
                            _context.StockArticulos.Update(stockArt);
                        }

                        detalle.Anulada = true;
                        _context.DetallesFacturaVentasArticulo.Update(detalle);
                    }
                }

                var detallesMedicamentosExistentes = await _context.DetallesFacturaVentasMedicamento
                    .Where(d => d.codFacturaVenta == factura.CodFacturaVenta && !d.Anulada)
                    .ToListAsync();

                if (detalleMedicamentos != null)
                {
                    var listaDetalleMedicamentos = detalleMedicamentos.ToList();

                    foreach (var detalleNuevo in listaDetalleMedicamentos)
                    {
                        var precioMed = await _context.Medicamentos
                            .AsNoTracking()
                            .Where(m => m.CodMedicamento == detalleNuevo.codMedicamento)
                            .Select(m => (decimal?)m.PrecioUnitario)
                            .FirstOrDefaultAsync();

                        if (precioMed == null)
                        {
                            throw new ArgumentException($"No existe el medicamento con código '{detalleNuevo.codMedicamento}'.");
                        }

                        var stockMed = await _context.StockMedicamentos
                            .Where(s => s.CodMedicamento == detalleNuevo.codMedicamento && s.CodSucursal == facturaExistente.CodSucursal)
                            .FirstOrDefaultAsync();

                        if (stockMed == null)
                        {
                            throw new ArgumentException($"No existe stock registrado para el medicamento con código '{detalleNuevo.codMedicamento}' en la sucursal N°{facturaExistente.CodSucursal}.");
                        }

                        var detalleExistente = detallesMedicamentosExistentes.FirstOrDefault(d => d.codMedicamento == detalleNuevo.codMedicamento);

                        if (detalleExistente != null)
                        {
                            // ACTUALIZAR detalle existente
                            int diferenciaCantidad = detalleNuevo.cantidad - detalleExistente.cantidad;

                            if (diferenciaCantidad > 0 && stockMed.Cantidad < diferenciaCantidad)
                            {
                                throw new InvalidOperationException(
                                    $"No hay suficiente stock para aumentar la cantidad del medicamento '{detalleNuevo.codMedicamento}'. " +
                                    $"Stock disponible: {stockMed.Cantidad}, cantidad adicional requerida: {diferenciaCantidad}.");
                            }

                            stockMed.Cantidad -= diferenciaCantidad;
                            _context.StockMedicamentos.Update(stockMed);

                            detalleExistente.cantidad = detalleNuevo.cantidad;
                            detalleExistente.codCobertura = detalleNuevo.codCobertura;
                            detalleExistente.precioUnitario = precioMed.Value;
                            _context.DetallesFacturaVentasMedicamento.Update(detalleExistente);
                            totalFactura += detalleExistente.precioUnitario * detalleExistente.cantidad;
                        }
                        else
                        {
                            // CREAR nuevo detalle
                            if (stockMed.Cantidad < detalleNuevo.cantidad)
                            {
                                throw new InvalidOperationException(
                                    $"No hay suficiente stock para el medicamento con código '{detalleNuevo.codMedicamento}'. " +
                                    $"Stock disponible: {stockMed.Cantidad}, cantidad solicitada: {detalleNuevo.cantidad}.");
                            }

                            stockMed.Cantidad -= detalleNuevo.cantidad;
                            _context.StockMedicamentos.Update(stockMed);

                            detalleNuevo.codFacturaVenta = factura.CodFacturaVenta;
                            detalleNuevo.precioUnitario = precioMed.Value;
                            totalFactura += detalleNuevo.precioUnitario * detalleNuevo.cantidad;
                            await _context.DetallesFacturaVentasMedicamento.AddAsync(detalleNuevo);
                        }
                    }

                    // ANULAR detalles que ya no vienen y DEVOLVER STOCK
                    var codigosMedicamentosNuevos = listaDetalleMedicamentos.Select(d => d.codMedicamento).ToList();
                    var detallesAEliminar = detallesMedicamentosExistentes
                        .Where(d => !codigosMedicamentosNuevos.Contains(d.codMedicamento))
                        .ToList();

                    foreach (var detalle in detallesAEliminar)
                    {
                        var stockMed = await _context.StockMedicamentos
                            .Where(s => s.CodMedicamento == detalle.codMedicamento && s.CodSucursal == facturaExistente.CodSucursal)
                            .FirstOrDefaultAsync();

                        if (stockMed != null)
                        {
                            stockMed.Cantidad += detalle.cantidad;
                            _context.StockMedicamentos.Update(stockMed);
                        }

                        detalle.Anulada = true;
                        _context.DetallesFacturaVentasMedicamento.Update(detalle);
                    }
                }
                else
                {
                    // Si no vienen detalles de medicamentos, anular TODOS y devolver stock
                    foreach (var detalle in detallesMedicamentosExistentes)
                    {
                        var stockMed = await _context.StockMedicamentos
                            .Where(s => s.CodMedicamento == detalle.codMedicamento && s.CodSucursal == facturaExistente.CodSucursal)
                            .FirstOrDefaultAsync();

                        if (stockMed != null)
                        {
                            stockMed.Cantidad += detalle.cantidad;
                            _context.StockMedicamentos.Update(stockMed);
                        }

                        detalle.Anulada = true;
                        _context.DetallesFacturaVentasMedicamento.Update(detalle);
                    }
                }

                // Actualizar total de la factura
                facturaExistente.Total = totalFactura;
                _context.FacturasVenta.Update(facturaExistente);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                // Si hay cualquier error, revertir todos los cambios
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteFacturaAsync(int codFacturaVenta, int usuarioId)
        {
            // Verificar que la factura existe
            var facturaExistente = await _context.FacturasVenta.FirstOrDefaultAsync(f => f.CodFacturaVenta == codFacturaVenta);
            if (facturaExistente == null)
                throw new ArgumentException($"No existe una factura con Codigo '{codFacturaVenta}'.");

            // Verificar que el usuario tiene acceso a la sucursal
            var tieneAcceso = await _context.Grupsucursales
                .AnyAsync(gs => gs.CodUsuario == usuarioId && gs.CodSucursal == facturaExistente.CodSucursal && gs.Activo);

            if (!tieneAcceso)
                return false;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Anular la factura de venta
                facturaExistente.Anulada = true;
                _context.FacturasVenta.Update(facturaExistente);

                // Anular los detalles de medicamentos y DEVOLVER STOCK
                var detallesMedicamentos = await _context.DetallesFacturaVentasMedicamento
                    .Where(d => d.codFacturaVenta == codFacturaVenta && !d.Anulada)
                    .ToListAsync();

                foreach (var detalle in detallesMedicamentos)
                {
                    // Devolver el stock del medicamento
                    var stockMed = await _context.StockMedicamentos
                        .Where(s => s.CodMedicamento == detalle.codMedicamento && s.CodSucursal == facturaExistente.CodSucursal)
                        .FirstOrDefaultAsync();

                    if (stockMed != null)
                    {
                        stockMed.Cantidad += detalle.cantidad;
                        _context.StockMedicamentos.Update(stockMed);
                    }

                    detalle.Anulada = true;
                    _context.DetallesFacturaVentasMedicamento.Update(detalle);
                }

                // Anular los detalles de artículos y DEVOLVER STOCK
                var detallesArticulos = await _context.DetallesFacturaVentasArticulo
                    .Where(d => d.codFacturaVenta == codFacturaVenta && !d.Anulada)
                    .ToListAsync();

                foreach (var detalle in detallesArticulos)
                {
                    // Devolver el stock del artículo
                    var stockArt = await _context.StockArticulos
                        .Where(s => s.CodArticulo == detalle.codArticulo && s.CodSucursal == facturaExistente.CodSucursal)
                        .FirstOrDefaultAsync();

                    if (stockArt != null)
                    {
                        stockArt.Cantidad += detalle.cantidad;
                        _context.StockArticulos.Update(stockArt);
                    }

                    detalle.Anulada = true;
                    _context.DetallesFacturaVentasArticulo.Update(detalle);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<FormasPago>> GetFormasPagoAsync()
        {
            return await _context.FormasPagos.ToListAsync();
        }
    }
}
