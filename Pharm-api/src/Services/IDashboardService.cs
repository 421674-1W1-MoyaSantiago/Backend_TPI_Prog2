using Pharm_api.DTOs;

namespace Pharm_api.Services
{
    public interface IDashboardService
    {
        Task<IEnumerable<ObraSocialPorcentajeVentasDto>> GetPorcentajeVentasXObraSocialAsync();
        Task<IEnumerable<IngresosXMesDto>> GetIngresosPorMesAnioActual();
    }
}
