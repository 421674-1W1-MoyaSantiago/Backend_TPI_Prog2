using Pharm_api.Models;
using Pharm_api.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pharm_api.DTOs;

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

        public async Task<bool> CreateFacturaForUsuarioAsync(CreateFacturaVentaDto createDto, int usuarioId)
        {
            var factura = new FacturasVentum
            {
                // Mapear propiedades desde createDto
                CodSucursal = createDto.CodSucursal,
                // ...otras propiedades...
            };
            return await _repository.CreateFacturaForUsuarioAsync(factura, usuarioId);
        }

        public async Task<List<DetalleMedicamentoDto>> GetDetallesMedicamentoAsync(int facturaId)
        {
            return await _repository.GetDetallesMedicamentoAsync(facturaId);
        }

        public async Task<List<DetalleFacturaBaseDto>> GetDetallesUnificadosAsync(int facturaId)
        {
            return await _repository.GetDetallesUnificadosAsync(facturaId);
        }
    }
}