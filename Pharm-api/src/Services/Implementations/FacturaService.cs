using Microsoft.EntityFrameworkCore;
using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharm_api.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _repository;

        public FacturaService(IFacturaRepository repository)
        {
            _repository = repository;
        }

        public async Task<FacturaVentaDto?> GetFacturaConDetallesAsync(int codFacturaVenta)
        {
            return await _repository.GetFacturaConDetallesAsync(codFacturaVenta);
        }

        // ...solo métodos por usuario...

        public async Task<IEnumerable<FacturaVentaDto>> GetFacturasByUsuarioAsync(int usuarioId)
        {
            var facturas = await _repository.GetFacturasByUsuarioAsync(usuarioId);
            var result = new List<FacturaVentaDto>();
            foreach (var f in facturas) {
                // Obtener detalles unificados
                var detallesUnificados = await _repository.GetDetallesUnificadosAsync(f.CodFacturaVenta);
                
                result.Add(new FacturaVentaDto {
                    CodFacturaVenta = f.CodFacturaVenta,
                    Fecha = f.Fecha,
                    CodEmpleado = f.CodEmpleado,
                    NombreEmpleado = f.CodEmpleadoNavigation != null ?
                        $"{f.CodEmpleadoNavigation.NomEmpleado} {f.CodEmpleadoNavigation.ApeEmpleado}" : "",
                    CodCliente = f.CodCliente,
                    NombreCliente = f.CodClienteNavigation != null ?
                        $"{f.CodClienteNavigation.NomCliente} {f.CodClienteNavigation.ApeCliente}" : "",
                    CodSucursal = f.CodSucursal,
                    NombreSucursal = f.CodSucursalNavigation?.NomSucursal ?? "",
                    CodFormaPago = f.CodFormaPago,
                    MetodoPago = f.CodFormaPagoNavigation?.Metodo ?? "",
                    Total = f.Total ?? 0,
                    Detalles = detallesUnificados
                });
            }
            return result;
        }

        public async Task<bool> CreateFacturaAsync(CreateFacturaVentaDto createDto, int usuarioId)
        {
            if (createDto.DetalleArticulos == null && createDto.DetalleMedicamentos == null)
            {
                throw new ArgumentException("La factura debe contener al menos un detalle de artículo o medicamento.");
            }

            var factura = new FacturasVentum
            {
                // Mapear propiedades desde createDto
                CodFormaPago = createDto.CodFormaPago,
                CodSucursal  = createDto.CodSucursal,
                CodEmpleado  = createDto.CodEmpleado,
                CodCliente   = createDto.CodCliente,
                Fecha        = DateTime.Now
            };

            List<DetallesFacturaVentasArticulo>? detallesArticulos = null;
            if (createDto.DetalleArticulos != null && createDto.DetalleArticulos.Any())
            {
                detallesArticulos = new List<DetallesFacturaVentasArticulo>();
                foreach (var detalle in createDto.DetalleArticulos)
                {
                    if (detalle.Cantidad <= 0) throw new ArgumentException("La cantidad no puede ser menor o igual a 0.");
                    detallesArticulos.Add(new DetallesFacturaVentasArticulo
                    {
                        codArticulo = detalle.CodArticulo,
                        cantidad = detalle.Cantidad
                    });
                }
            }

            List<DetallesFacturaVentasMedicamento>? detallesMedicamentos = null;
            if (createDto.DetalleMedicamentos != null && createDto.DetalleMedicamentos.Any())
            {
                detallesMedicamentos = new List<DetallesFacturaVentasMedicamento>();
                foreach (var detalle in createDto.DetalleMedicamentos)
                {
                    if (detalle.Cantidad <= 0) throw new ArgumentException("La cantidad no puede ser menor o igual a 0.");
                    detallesMedicamentos.Add(new DetallesFacturaVentasMedicamento
                    {
                        codMedicamento = detalle.CodMedicamento,
                        codCobertura = detalle.CodCobertura,
                        cantidad = detalle.Cantidad
                    });
                }
            }

            return await _repository.CreateFacturaAsync(factura, usuarioId, detallesArticulos, detallesMedicamentos);
        }

        public async Task<bool> EditFacturaAsync(EditFacturaVentaDto editDto, int codFacturaVenta, int usuarioId)
        {
            if (editDto.DetalleArticulos == null && editDto.DetalleMedicamentos == null)
            {
                throw new ArgumentException("La factura debe contener al menos un detalle de artículo o medicamento.");
            }

            var factura = new FacturasVentum
            {
                CodFacturaVenta = codFacturaVenta
            };

            List<DetallesFacturaVentasArticulo>? detallesArticulos = null;
            if (editDto.DetalleArticulos != null && editDto.DetalleArticulos.Any())
            {
                detallesArticulos = new List<DetallesFacturaVentasArticulo>();
                foreach (var detalle in editDto.DetalleArticulos)
                {
                    detallesArticulos.Add(new DetallesFacturaVentasArticulo
                    {
                        codArticulo = detalle.CodArticulo,
                        cantidad = detalle.Cantidad
                    });
                }
            }

            List<DetallesFacturaVentasMedicamento>? detallesMedicamentos = null;
            if (editDto.DetalleMedicamentos != null && editDto.DetalleMedicamentos.Any())
            {
                detallesMedicamentos = new List<DetallesFacturaVentasMedicamento>();
                foreach (var detalle in editDto.DetalleMedicamentos)
                {
                    detallesMedicamentos.Add(new DetallesFacturaVentasMedicamento
                    {
                        codMedicamento = detalle.CodMedicamento,
                        codCobertura = detalle.CodCobertura,
                        cantidad = detalle.Cantidad
                    });
                }
            }

            return await _repository.EditFacturaAsync(factura, usuarioId, detallesArticulos, detallesMedicamentos);
        }

        public async Task<bool> DeleteFacturaAsync(int codFacturaVenta, int usuarioId)
        {
            return await _repository.DeleteFacturaAsync(codFacturaVenta, usuarioId);
        }

        public async Task<List<FormaPagoDto>> GetFormasPagoAsync()
        {
            IEnumerable<FormasPago> formasPago = await _repository.GetFormasPagoAsync();
            return formasPago.Select(fp => new FormaPagoDto
            {
                CodMetodoPago = fp.CodFormaPago,
                FormaPago = fp.Metodo
            }).ToList();
        }
    }
}