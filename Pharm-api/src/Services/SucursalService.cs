using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Repositories;

namespace Pharm_api.Services
{
    public class SucursalService : ISucursalService
    {
        private readonly ISucursalRepository _repository;

        public SucursalService(ISucursalRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SucursalDto>> GetAllSucursalesAsync()
        {
            IEnumerable<Sucursale> sucursales = await _repository.GetAllSucursalesAsync();
            return sucursales.Select(s => new SucursalDto
            {
                CodSucursal = s.CodSucursal,
                NomSucursal = s.NomSucursal
            });
        }
    }
}
