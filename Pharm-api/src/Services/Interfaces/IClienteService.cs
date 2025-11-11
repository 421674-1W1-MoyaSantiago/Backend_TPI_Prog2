using Pharm_api.DTOs;

namespace Pharm_api.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> GetClientesAsync();
    }
}
