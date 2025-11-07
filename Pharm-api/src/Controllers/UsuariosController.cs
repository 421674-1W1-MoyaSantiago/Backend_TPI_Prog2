using Microsoft.AspNetCore.Mvc;
using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Services;
using Microsoft.AspNetCore.Authorization;

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

    [HttpPost("create-from-auth")]
    public async Task<ActionResult> CreateUserFromAuthNotification([FromBody] CreateUserFromAuthDto dto)
    {
        try
        {
            // Redirigir al nuevo SyncController que maneja esto mejor
            // Este endpoint se mantiene por compatibilidad pero delegamos la lógica
            var syncService = HttpContext.RequestServices.GetRequiredService<IUserSyncService>();
            var result = await syncService.CreateUserFromAuthAsync(dto);

            if (result.Success)
            {
                return Ok(new { 
                    success = result.Success,
                    message = result.Message,
                    userId = result.UserId,
                    username = result.Username,
                    sucursalesAsignadas = result.SucursalesAsignadas,
                    totalSucursales = result.TotalSucursales
                });
            }
            else
            {
                return StatusCode(500, new {
                    success = result.Success,
                    message = result.Message,
                    error = "Error en sincronización"
                });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = $"Error al crear usuario desde Auth-api: {ex.Message}",
                error = ex.ToString()
            });
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
    
    [HttpGet("{username}/sucursales")]
    public async Task<ActionResult> GetSucursalesUsuario(string username)
    {
        try
        {
            var sucursales = await _usuarioService.GetSucursalesUsuarioAsync(username);
            return Ok(new { 
                Username = username,
                SucursalesAsignadas = sucursales,
                TotalSucursales = sucursales.Count(),
                Mensaje = sucursales.Any() ? "Usuario tiene sucursales asignadas" : "Usuario NO tiene sucursales asignadas"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener sucursales: {ex.Message}");
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<object>> GetAllUsuarios()
    {
        try
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            return Ok(new {
                success = true,
                totalUsuarios = usuarios.Count(),
                usuarios = usuarios,
                mensaje = $"Se encontraron {usuarios.Count()} usuarios en PharmDB"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = $"Error al obtener usuarios: {ex.Message}",
                error = ex.ToString()
            });
        }
    }

    [HttpGet("simple")]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuariosSimple()
    {
        try
        {
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener usuarios: {ex.Message}");
        }
    }

[Authorize]
[HttpGet("me")]
public async Task<ActionResult<object>> GetCurrentUser()
{
    var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
    var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    return Ok(new {
        UserId = userId,
        Username = username,
        Email = email
    });
}
}