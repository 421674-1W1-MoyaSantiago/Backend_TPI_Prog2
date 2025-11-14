
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pharm_api.DTOs;
using Pharm_api.Services;
using Microsoft.AspNetCore.Http.HttpResults;

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
                return NotFound($"No se encontro la factura con Codigo '{codFacturaVenta}'");
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
                var ok = await _facturaService.CreateFacturaAsync(createDto, userId);
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

        [Authorize]
        [HttpPut("{codFacturaVenta}")]
        public async Task<IActionResult> EditFactura([FromBody] EditFacturaVentaDto editDto, int codFacturaVenta)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();

                int userId = int.Parse(userIdClaim.Value);
                var ok = await _facturaService.EditFacturaAsync(editDto, codFacturaVenta, userId);
                if (!ok)
                    return BadRequest("No se pudo editar la factura");
                return Ok(new { Mensaje = "Factura editada exitosamente" });
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
        
        [Authorize]
        [HttpDelete("{codFacturaVenta}")]
        public async Task<IActionResult> DeleteFactura(int codFacturaVenta) 
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();

                int userId = int.Parse(userIdClaim.Value);
                bool result = await _facturaService.DeleteFacturaAsync(codFacturaVenta, userId);
                if (result)
                    return NoContent();
                else
                    return BadRequest();
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

        [Authorize]
        [HttpGet("formas-pago")]
        public async Task<ActionResult<List<FormaPagoDto>>> GetFormasPagoAsync()
        {
            try
            {
                List<FormaPagoDto> formasPago = await _facturaService.GetFormasPagoAsync();
                return Ok(formasPago);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}