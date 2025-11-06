using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;
using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public class SucursalRepository : ISucursalRepository
    {
        private readonly PharmDbContext _context;

        public SucursalRepository(PharmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sucursale>> GetAllSucursalesAsync()
        {
            return await _context.Sucursales.ToListAsync();
        }
    }
}
