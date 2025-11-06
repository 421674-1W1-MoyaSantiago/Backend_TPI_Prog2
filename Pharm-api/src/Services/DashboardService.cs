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

        public Task<IEnumerable<ObraSocialPorcentajeVentasDto>> GetPorcentajeVentasXObraSocialAsync()
        {
            return _repository.GetPorcentajeVentasXObraSocialAsync();
        }
    }
}
