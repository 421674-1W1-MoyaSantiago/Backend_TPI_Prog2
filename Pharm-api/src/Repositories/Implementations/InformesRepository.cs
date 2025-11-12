using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;
using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public class InformesRepository : IInformesRepository
    {
        private readonly PharmDbContext _context;
        public InformesRepository(PharmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ObrasSociale>> GetObrasSocialesAsync()
        {
            return await _context.ObrasSociales.ToListAsync();
        }

        public async Task<bool> DoObraSocialExists(string nombreObraSocial)
        {
            return await _context.ObrasSociales.AnyAsync(os => os.RazonSocial == nombreObraSocial);
        }

        // INFORMES

        public async Task<IEnumerable<TopMedicamentoYVendedorPorEstacionDto>> GetTopMedicamentoYVendedorPorEstacionAsync(string estacion)
        {
            return await _context.Database.SqlQuery<TopMedicamentoYVendedorPorEstacionDto>(
                $@"EXEC sp_TopMedicamentoYVendedorPorEstacion 
                   @Estacion = {estacion}").ToListAsync();
        }

        public async Task<IEnumerable<VentasMedicamentosObraSocialDto>> GetVentasMedicamentosObraSocialAsync(DateTime fechaInicio, DateTime fechaFin, string? nombreObraSocial = null)
        {
            return await _context.Database.SqlQuery<VentasMedicamentosObraSocialDto>(
                $@"EXEC sp_Ventas_Medicamentos_ObraSocial 
                   @ObraSocialNombre = {nombreObraSocial},
                   @FechaInicio = {fechaInicio}, 
                   @FechaFin = {fechaFin}").ToListAsync();
        }

        public async Task<IEnumerable<ComprasSuministrosConAutorizacionObraSocialDto>> GetComprasSuministrosConAutorizacionObraSocialAsync()
        {
            return await _context.Database.SqlQuery<ComprasSuministrosConAutorizacionObraSocialDto>(
                $@"EXEC sp_ComprasSuministrosConAutorizacionObraSocial").ToListAsync();
        }

        public async Task<IEnumerable<RecetasObraSocialEstadoDto>> GetRecetasObraSocialEstadoAsync(string? nombreObraSocial, string? estado)
        {
            return await _context.Database.SqlQuery<RecetasObraSocialEstadoDto>(
                $@"EXEC SP_RECETAS_OBRA_SOCIAL_ESTADO 
                   @obra_social = {nombreObraSocial}, 
                   @estado = {estado}
                ").ToListAsync();
        }

        public async Task<IEnumerable<ConsultarReintegrosObrasSocialesDto>> GetReintegrosObrasSocialesAsync(
            DateTime fechaInicio, DateTime fechaFin, string? nombreObraSocial = null, string? estado = null
        )
        {
            return await _context.Database.SqlQuery<ConsultarReintegrosObrasSocialesDto>(
                $@"EXEC sp_Consultar_Reintegros_Obras_Sociales 
                   @FechaInicio = {fechaInicio}, 
                   @FechaFin = {fechaFin},
                   @Estado = {estado}, 
                   @ObraSocialNombre = {nombreObraSocial}
                ").ToListAsync();
        }

        public async Task<IEnumerable<ReporteVentasPorObraSocialDto>> GetReporteVentasPorObraSocialAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Database.SqlQuery<ReporteVentasPorObraSocialDto>(
                $@"EXEC sp_ReporteVentasPorObraSocial
                   @FechaInicio = {fechaInicio}, 
                   @FechaFin = {fechaFin}
                ").ToListAsync();
        }
    }
}
