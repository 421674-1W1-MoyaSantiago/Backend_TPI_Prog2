using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharm_api.DTOs;
using Pharm_api.Services;

namespace Pharm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere autenticación para todos los endpoints
    public class InformesController : ControllerBase
    {
        private readonly IInformesService _informesService;
        public InformesController(IInformesService informesService)
        {
            _informesService = informesService;
        }

        // Endpoints auxiliares para filtros
        [HttpGet("obras-sociales")]
        public async Task<ActionResult<IEnumerable<ObraSocialDto>>> GetObrasSociales()
        {
            try
            {
                IEnumerable<ObraSocialDto> obrasSociales = await _informesService.GetObrasSocialesAsync();
                return Ok(obrasSociales);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("estados-autorizaciones")]
        public ActionResult<IEnumerable<string>> GetEstadosAutorizaciones()
        {
            try
            {
                IEnumerable<string> estados = _informesService.GetEstadosAutorizaciones();
                return Ok(estados);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        // Informes

        [HttpGet("ventas-medicamentos-obra-social")]
        public async Task<IActionResult> GetVentasMedicamentosObraSocial(
            [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin, [FromQuery] string? nombreObraSocial = null
            )
        {
            try
            {
                IEnumerable<VentasMedicamentosObraSocialDto> resultados = await _informesService.GetVentasMedicamentosObraSocialAsync(fechaInicio, fechaFin, nombreObraSocial);
                return Ok(resultados);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor." );
            }
        }

        [HttpGet("top-medicamento-vendedor-por-estacion")]
        public async Task<ActionResult<IEnumerable<TopMedicamentoYVendedorPorEstacionDto>>> GetTopMedicamentoYVendedorPorEstacion([FromQuery] string estacion)
        {
            try
            {
                IEnumerable<TopMedicamentoYVendedorPorEstacionDto> resultados = await _informesService.GetTopMedicamentoYVendedorPorEstacionAsync(estacion);
                return Ok(resultados);
            } 
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("compras-suministros-con-autorizacion-obra-social")]
        public async Task<ActionResult<IEnumerable<ComprasSuministrosConAutorizacionObraSocialDto>>> GetComprasSuministrosConAutorizacionObraSocial()
        {
            try
            {
                IEnumerable<ComprasSuministrosConAutorizacionObraSocialDto> resultados = await _informesService.GetComprasSuministrosConAutorizacionObraSocialAsync();
                return Ok(resultados);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("recetas-obra-social-y-estado")]
        public async Task<ActionResult<IEnumerable<RecetasObraSocialEstadoDto>>> GetRecetasObraSocialEstado(
            [FromQuery] string? obraSocial, [FromQuery] string? estado
        )
        {
            try
            {
                IEnumerable<RecetasObraSocialEstadoDto> resultados = await _informesService.GetRecetasObraSocialEstadoAsync(obraSocial, estado);
                return Ok(resultados);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpGet("reintegros-obras-sociales")]
        public async Task<ActionResult<IEnumerable<ConsultarReintegrosObrasSocialesDto>>> GetReintegrosObrasSociales(
            [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin, [FromQuery] string? nombreObraSocial = null, [FromQuery] string? estado = null
            )
        {
            try
            {
                IEnumerable<ConsultarReintegrosObrasSocialesDto> resultados = await _informesService.GetReintegrosObrasSocialesAsync(fechaInicio, fechaFin, nombreObraSocial, estado);
                return Ok(resultados);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpGet("reporte-ventas-por-obra-social")]
        public async Task<ActionResult<IEnumerable<ReporteVentasPorObraSocialDto>>> GetReporteVentasPorObraSocial(
            [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                IEnumerable<ReporteVentasPorObraSocialDto> resultados = await _informesService.GetReporteVentasPorObraSocialAsync(fechaInicio, fechaFin);
                return Ok(resultados);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
