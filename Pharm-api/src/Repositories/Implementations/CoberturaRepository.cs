using Pharm_api.Models;
using Pharm_api.DTOs;
using Pharm_api.Repositories.Interfaces;
using Pharm_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharm_api.Repositories.Implementations
{
    public class CoberturaRepository : ICoberturaRepository
    {
        private readonly PharmDbContext _context;
        public CoberturaRepository(PharmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CoberturaDto>> GetCoberturasBySucursalAsync(int codSucursal)
        {
            var codLocalidad = _context.Sucursales
                .Where(s => s.CodSucursal == codSucursal)
                .Select(s => s.CodLocalidad)
                .FirstOrDefault();

            var coberturas = await _context.Coberturas
                .Where(c => c.CodLocalidad == codLocalidad)
                .Include(c => c.CodClienteNavigation)
                .Include(c => c.CodObraSocialNavigation)
                .Include(c => c.CodDescuentoNavigation)
                .ToListAsync();

            return coberturas.Select(c => new CoberturaDto
            {
                CodCobertura = c.CodCobertura,
                IdCliente = c.CodClienteNavigation?.CodCliente ?? 0,
                NombreCliente = c.CodClienteNavigation?.NomCliente,
                NombreObraSocial = c.CodObraSocialNavigation?.RazonSocial,
                NombreDescuento = c.CodDescuentoNavigation != null ? $"{c.CodDescuentoNavigation.PorcentajeDescuento}%" : null,
                FechaInicio = c.FechaInicio,
                FechaFin = c.FechaFin
            });
        }

        public async Task<IEnumerable<CoberturaDto>> GetCoberturasByClienteAsync(int idCliente)
        {
            var coberturas = await _context.Coberturas
                .Where(c => c.CodCliente == idCliente)
                .Include(c => c.CodClienteNavigation)
                .Include(c => c.CodObraSocialNavigation)
                .Include(c => c.CodDescuentoNavigation)
                .ToListAsync();

            return coberturas.Select(c => new CoberturaDto
            {
                CodCobertura = c.CodCobertura,
                IdCliente = c.CodClienteNavigation?.CodCliente ?? 0,
                NombreCliente = c.CodClienteNavigation?.NomCliente,
                NombreObraSocial = c.CodObraSocialNavigation?.RazonSocial,
                NombreDescuento = c.CodDescuentoNavigation != null ? $"{c.CodDescuentoNavigation.PorcentajeDescuento}%" : null,
                FechaInicio = c.FechaInicio,
                FechaFin = c.FechaFin
            });
        }
    }
}
