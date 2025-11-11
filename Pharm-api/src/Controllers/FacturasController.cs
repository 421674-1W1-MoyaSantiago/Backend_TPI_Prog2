
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pharm_api.DTOs;
using Pharm_api.Services;

namespace Pharm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase
    {
 

        private readonly IFacturaService _facturaService;
        private readonly IUsuarioService _usuarioService;

        public FacturasController(IFacturaService facturaService, IUsuarioService usuarioService)
        {
            _facturaService = facturaService;
            _usuarioService = usuarioService;
        }


        [Authorize]
        [HttpGet("{codFacturaVenta}/detalles")]
        public async Task<ActionResult<FacturaVentaDto>> GetFacturaConDetalles(int codFacturaVenta)
        {
            var factura = await _facturaService.GetFacturaConDetallesAsync(codFacturaVenta);
            if (factura == null)
                return NotFound();
            return Ok(factura);
        }

        [Authorize]
        [HttpGet("mis-facturas")]
        public async Task<ActionResult<IEnumerable<FacturaVentaDto>>> GetMisFacturas()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);
            var facturas = await _facturaService.GetFacturasByUsuarioAsync(userId);
            return Ok(facturas);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFactura(CreateFacturaVentaDto createDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();

                int userId = int.Parse(userIdClaim.Value);
                var ok = await _facturaService.CreateFacturaForUsuarioAsync(createDto, userId);
                if (!ok)
                    return BadRequest("No se pudo crear la factura");
                return Ok(new { Mensaje = "Factura creada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Forbid(ex.Message);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        // Endpoint de debug para verificar medicamentos en BD
        [HttpGet("debug/medicamentos/{facturaId}")]
        public async Task<ActionResult> DebugMedicamentos(int facturaId)
        {
            var medicamentos = await _facturaService.GetDetallesMedicamentoAsync(facturaId);
            return Ok(new { FacturaId = facturaId, Medicamentos = medicamentos });
        }

        // Endpoint de debug para verificar detalles unificados
        [HttpGet("debug/detalles/{facturaId}")]
        public async Task<ActionResult> DebugDetalles(int facturaId)
        {
            var detalles = await _facturaService.GetDetallesUnificadosAsync(facturaId);
            return Ok(new { FacturaId = facturaId, Detalles = detalles });
        }

    }
}