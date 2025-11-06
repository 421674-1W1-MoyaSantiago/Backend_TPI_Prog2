using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;
using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public class DashboardRepository
    {
        private readonly PharmDbContext _context;

        public DashboardRepository(PharmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ObraSocialPorcentajeVentasDto>> GetObraSociaPorcentajeVentasAsync()
        {
            return await _context.Database.SqlQuery<ObraSocialPorcentajeVentasDto>(
                $"""
                SELECT 
                	OS.razonSocial as NomObraSocial,
                	sum(FV.total) / (SELECT sum(total) FROM FacturasVenta FV WHERE year(fecha) = year(getdate())) as Porcentaje 
                FROM FacturasVenta FV
                JOIN Clientes C ON C.cod_Cliente = FV.codCliente
                JOIN Obras_Sociales OS ON OS.cod_Obra_Social = C.cod_Obra_Social
                WHERE year(FV.fecha) = year(getdate())
                GROUP BY OS.razonSocial
                ORDER BY Porcentaje DESC
                """).ToListAsync();
                
        }
    }
}
