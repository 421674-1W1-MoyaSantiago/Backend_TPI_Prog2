using Pharm_api.DTOs;

namespace Pharm_api.Services
{
    public interface IInformesService
    {
        Task<IEnumerable<ObraSocialDto>> GetObrasSocialesAsync();
        IEnumerable<string> GetEstadosAutorizaciones();
        Task<IEnumerable<VentasMedicamentosObraSocialDto>> GetVentasMedicamentosObraSocialAsync(DateTime fechaInicio, DateTime fechaFin, string? nombreObraSocial = null);
        Task<IEnumerable<TopMedicamentoYVendedorPorEstacionDto>> GetTopMedicamentoYVendedorPorEstacionAsync(string estacion);
        Task<IEnumerable<ComprasSuministrosConAutorizacionObraSocialDto>> GetComprasSuministrosConAutorizacionObraSocialAsync();
        Task<IEnumerable<RecetasObraSocialEstadoDto>> GetRecetasObraSocialEstadoAsync(string? nombreObraSocial, string? estado);
        Task<IEnumerable<ConsultarReintegrosObrasSocialesDto>> GetReintegrosObrasSocialesAsync(
            DateTime fechaInicio, DateTime fechaFin, string? nombreObraSocial = null, string? estado = null);
        Task<IEnumerable<ReporteVentasPorObraSocialDto>> GetReporteVentasPorObraSocialAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}
