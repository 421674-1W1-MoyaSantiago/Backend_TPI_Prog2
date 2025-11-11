using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharm_api.DTOs;
using Pharm_api.Services.Interfaces;

namespace Pharm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere autenticación para todos los endpoints
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientesAsync()
        {
            try
            {
                IEnumerable<ClienteDto> clientes = await _clienteService.GetClientesAsync();
                return Ok(clientes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }

        }
    }
}
