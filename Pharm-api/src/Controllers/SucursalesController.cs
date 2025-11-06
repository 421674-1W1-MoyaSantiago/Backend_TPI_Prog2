using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharm_api.DTOs;
using Pharm_api.Services;
using System.Threading.Tasks;

namespace Pharm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere autenticación para todos los endpoints
    public class SucursalesController : ControllerBase
    {
        private readonly ISucursalService _sucursalService;
        public SucursalesController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SucursalDto>>> GetSucursales()
        {
            try
            {
                var sucursales = await _sucursalService.GetAllSucursalesAsync();
                return Ok(sucursales);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
