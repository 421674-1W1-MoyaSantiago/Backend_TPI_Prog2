using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pharm_api.DTOs;
using Pharm_api.Services;
using Pharm_api.Models;

namespace Pharm_api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requiere autenticación para todos los endpoints
public class EmpleadosController : ControllerBase
{
    private readonly IEmpleadoService _empleadoService;
    private readonly IUsuarioService _usuarioService;

    public EmpleadosController(IEmpleadoService empleadoService, IUsuarioService usuarioService)
    {
        _empleadoService = empleadoService;
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetEmpleados()
    {
        var userId = _usuarioService.GetUserIdFromToken(HttpContext);
        if (userId == null)
            return Unauthorized();

        var empleados = await _empleadoService.GetEmpleadosByUsuarioAsync(userId.Value);
        return Ok(empleados);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmpleadoDto>> GetEmpleado(int id)
    {
        var userId = _usuarioService.GetUserIdFromToken(HttpContext);
        if (userId == null)
            return Unauthorized();

        var empleado = await _empleadoService.GetEmpleadoByIdAsync(id, userId.Value);
        if (empleado == null)
            return NotFound();

        return Ok(empleado);
    }

    [HttpPost]
    public async Task<ActionResult<EmpleadoDto>> CreateEmpleado(CreateEmpleadoDto createDto)
    {
        try
        {
            var userId = _usuarioService.GetUserIdFromToken(HttpContext);
            if (userId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var empleado = await _empleadoService.CreateEmpleadoAsync(createDto, userId.Value);
            if (empleado == null)
                return BadRequest("No se pudo crear el empleado");

            return CreatedAtAction(nameof(GetEmpleado), new { id = empleado.CodEmpleado }, empleado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmpleadoDto>> UpdateEmpleado(int id, UpdateEmpleadoDto updateDto)
    {
        try
        {
            var userId = _usuarioService.GetUserIdFromToken(HttpContext);
            if (userId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var empleado = await _empleadoService.UpdateEmpleadoAsync(id, updateDto, userId.Value);
            if (empleado == null)
                return NotFound("Empleado no encontrado");

            return Ok(empleado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmpleado(int id)
    {
        try
        {
            var userId = _usuarioService.GetUserIdFromToken(HttpContext);
            if (userId == null)
                return Unauthorized();

            var deleted = await _empleadoService.DeleteEmpleadoAsync(id, userId.Value);
            if (!deleted)
                return NotFound("Empleado no encontrado");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Error interno del servidor");
        }
    }

    // Endpoints auxiliares para formularios
    [HttpGet("tipos-empleado")]
    public async Task<ActionResult<IEnumerable<TiposEmpleado>>> GetTiposEmpleado()
    {
        var tipos = await _empleadoService.GetTiposEmpleadoAsync();
        return Ok(tipos);
    }

    [HttpGet("tipos-documento")]
    public async Task<ActionResult<IEnumerable<TiposDocumento>>> GetTiposDocumento()
    {
        var tipos = await _empleadoService.GetTiposDocumentoAsync();
        return Ok(tipos);
    }

    [HttpGet("mis-sucursales")]
    public async Task<ActionResult<IEnumerable<Sucursale>>> GetMisSucursales()
    {
        var userId = _usuarioService.GetUserIdFromToken(HttpContext);
        if (userId == null)
            return Unauthorized();

        var sucursales = await _empleadoService.GetSucursalesByUsuarioAsync(userId.Value);
        return Ok(sucursales);
    }
}