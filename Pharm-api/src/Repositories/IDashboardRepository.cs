using Pharm_api.DTOs;

namespace Pharm_api.Repositories
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<ObraSocialPorcentajeVentasDto>> GetPorcentajeVentasXObraSocialAsync();
        Task<IEnumerable<IngresosXMesDto>> GetIngresosPorMesAnioActual();
    }
}
