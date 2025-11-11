using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;
using Pharm_api.Models;
using Pharm_api.Repositories.Interfaces;

namespace Pharm_api.Repositories.Implementations
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly PharmDbContext _context;

        public ClienteRepository(PharmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetClientesAsync()
        {
            return await _context.Clientes.ToListAsync();
        }
    }
}
