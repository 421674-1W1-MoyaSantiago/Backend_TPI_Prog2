using Microsoft.AspNetCore.Mvc;
using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Services;

namespace Pharm_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IJwtService _jwtService;

    public UsuariosController(IUsuarioService usuarioService, IJwtService jwtService)
    {
        _usuarioService = usuarioService;
        _jwtService = jwtService;
    }

        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioByUsername(string username)
        {
            var usuario = await _usuarioService.GetByUsernameAsync(username);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });
            return Ok(usuario);
        }

    [HttpPost]
    public async Task<ActionResult> CreateUsuarioFromAuth(CreateUsuarioDto createDto)
    {
        try
        {
            var usuario = await _usuarioService.CreateUsuarioAsync(createDto);
            return Ok(new { Mensaje = "Usuario creado en Pharm-api", Username = usuario.Username });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al crear usuario: {ex.Message}");
        }
    }


    [HttpPost("{userId}/sucursales")]
    public async Task<ActionResult> AsignarSucursales(string userId, AsignarSucursalesDto dto)
    {
        try
        {
            await _usuarioService.AsignarSucursalesAsync(userId, dto.Sucursales);
            return Ok(new { Mensaje = "Sucursales asignadas correctamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al asignar sucursales: {ex.Message}");
        }
    }


    [HttpGet("{userId}/sucursales")]
    public async Task<ActionResult> GetSucursalesUsuario(string userId)
    {
        try
        {
            var sucursales = await _usuarioService.GetSucursalesUsuarioAsync(userId);
            return Ok(sucursales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener sucursales: {ex.Message}");
        }
    }


    [HttpGet("generate-token/{username}")]
    public async Task<IActionResult> GenerateToken(string username)
    {
        var user = await _usuarioService.GetByUsernameAsync(username);
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }

        var userModel = new Usuario
        {
            CodUsuario = user.Id,
            Username = user.Username,
            Email = user.Email,
        };

        var token = _jwtService.GenerateToken(userModel);
        return Ok(new { token });
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
    {
        var usuarios = await _usuarioService.GetAllUsuariosAsync();
        return Ok(usuarios);
    }
}