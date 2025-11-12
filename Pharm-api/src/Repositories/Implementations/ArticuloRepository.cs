using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Repositories.Interfaces;
using Pharm_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharm_api.Repositories.Implementations
{
    public class ArticuloRepository : IArticuloRepository
    {
        private readonly PharmDbContext _context;
        public ArticuloRepository(PharmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticuloDto>> GetAllAsync()
        {
            var articulos = await _context.Articulos
                .Include(a => a.CodProveedorNavigation)
                .Include(a => a.CodCategoriaArticuloNavigation)
                .ToListAsync();

            return articulos.Select(a => new ArticuloDto
            {
                CodArticulo = a.CodArticulo,
                CodBarra = a.CodBarra,
                Descripcion = a.Descripcion,
                PrecioUnitario = a.PrecioUnitario
            });
        }

        public async Task<ArticuloDto?> GetByIdAsync(int id)
        {
            var a = await _context.Articulos
                .Include(x => x.CodProveedorNavigation)
                .Include(x => x.CodCategoriaArticuloNavigation)
                .FirstOrDefaultAsync(x => x.CodArticulo == id);
            if (a == null) return null;
            return new ArticuloDto
            {
                CodArticulo = a.CodArticulo,
                CodBarra = a.CodBarra,
                Descripcion = a.Descripcion,
                PrecioUnitario = a.PrecioUnitario
            };
        }

        public async Task<IEnumerable<ArticuloDto>> GetBySucursalAsync(int codSucursal)
        {
            var articulos = await _context.Articulos
                .Join(_context.StockMedicamentos,
                    articulo => articulo.CodArticulo,
                    stock => stock.CodMedicamento, // Asumiendo que CodMedicamento se usa para ambos
                    (articulo, stock) => new { articulo, stock })
                .Where(asoc => asoc.stock.CodSucursal == codSucursal && asoc.stock.Cantidad > 0)
                .Select(asoc => new ArticuloDto
                {
                    CodArticulo = asoc.articulo.CodArticulo,
                    CodBarra = asoc.articulo.CodBarra,
                    Descripcion = asoc.articulo.Descripcion,
                    PrecioUnitario = asoc.articulo.PrecioUnitario
                })
                .ToListAsync();
            return articulos;
        }
    }
}

