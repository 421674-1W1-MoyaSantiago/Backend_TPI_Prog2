using Pharm_api.Models;

namespace Pharm_api.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetClientesAsync();
    }
}
