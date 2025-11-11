using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;
using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly PharmDbContext _context;

        public DashboardRepository(PharmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IngresosXMesDto>> GetIngresosPorMesAnioActual()
        {
            return await _context.Database.SqlQuery<IngresosXMesDto>(
                $"EXEC sp_IngresosPorMesAnioActual").ToListAsync();
        }

        public async Task<IEnumerable<ObraSocialPorcentajeVentasDto>> GetPorcentajeVentasXObraSocialAsync()
        {
            return await _context.Database.SqlQuery<ObraSocialPorcentajeVentasDto>(
                $"EXEC sp_PorcentajeVentasXObraSocial")
                .ToListAsync();
        }
    }
}
