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
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("porcentaje-ventas-obra-social")]
        public async Task<ActionResult<IEnumerable<ObraSocialPorcentajeVentasDto>>> GetPorcentajeVentas()
        {
            try
            {
                IEnumerable<ObraSocialPorcentajeVentasDto> porcentajes = await _dashboardService.GetPorcentajeVentasXObraSocialAsync();
                return Ok(porcentajes);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("ingresos-por-mes-anio-actual")]
        public async Task<ActionResult<IEnumerable<IngresosXMesDto>>> GetIngresosPorMes()
        {
            try
            {
                IEnumerable<IngresosXMesDto> ingresos = await _dashboardService.GetIngresosPorMesAnioActual();
                return Ok(ingresos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
