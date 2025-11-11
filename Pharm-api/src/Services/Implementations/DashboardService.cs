using Pharm_api.DTOs;
using Pharm_api.Repositories;

namespace Pharm_api.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IngresosXMesDto>> GetIngresosPorMesAnioActual()
        {
            return await _repository.GetIngresosPorMesAnioActual();
        }

        public async Task<IEnumerable<ObraSocialPorcentajeVentasDto>> GetPorcentajeVentasXObraSocialAsync()
        {
            return await _repository.GetPorcentajeVentasXObraSocialAsync();
        }
    }
}
