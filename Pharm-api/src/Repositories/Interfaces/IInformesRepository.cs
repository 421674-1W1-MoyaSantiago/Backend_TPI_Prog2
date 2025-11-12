using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public interface IInformesRepository
    {
        Task<IEnumerable<ObrasSociale>> GetObrasSocialesAsync();
        Task<bool> DoObraSocialExists(string nombreObraSocial); 
        // Informes
        Task<IEnumerable<VentasMedicamentosObraSocialDto>> GetVentasMedicamentosObraSocialAsync(DateTime fechaInicio, DateTime fechaFin, string? nombreObraSocial = null);
        Task<IEnumerable<TopMedicamentoYVendedorPorEstacionDto>> GetTopMedicamentoYVendedorPorEstacionAsync(string estacion);
        Task<IEnumerable<ComprasSuministrosConAutorizacionObraSocialDto>> GetComprasSuministrosConAutorizacionObraSocialAsync();
        Task<IEnumerable<RecetasObraSocialEstadoDto>> GetRecetasObraSocialEstadoAsync(string? nombreObraSocial, string? estado);
        Task<IEnumerable<ConsultarReintegrosObrasSocialesDto>> GetReintegrosObrasSocialesAsync(
            DateTime fechaInicio, DateTime fechaFin, string? nombreObraSocial = null, string? estado = null);
        Task<IEnumerable<ReporteVentasPorObraSocialDto>> GetReporteVentasPorObraSocialAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}
