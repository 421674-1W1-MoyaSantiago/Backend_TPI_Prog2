using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Repositories.Interfaces;
using Pharm_api.Services.Interfaces;

namespace Pharm_api.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;
        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ClienteDto>> GetClientesAsync()
        {
            IEnumerable<Cliente> clientes = await _repository.GetClientesAsync();
            return clientes.Select(c => new ClienteDto
            {
                CodCliente = c.CodCliente,
                NomCliente = c.NomCliente,
                ApeCliente = c.ApeCliente
            });
        }
    }
}
