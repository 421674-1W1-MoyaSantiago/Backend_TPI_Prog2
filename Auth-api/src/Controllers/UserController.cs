using Auth_api.Models;
using Auth_api.Services;
using Auth_api.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPharmApiService _pharmApiService;

        public UserController(IUserService userService, IPharmApiService pharmApiService)
        {
            _userService = userService;
            _pharmApiService = pharmApiService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // ✅ Verificar si el username ya existe
            var existingUser = await _userService.GetByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "El username ya está en uso" });
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userService.CreateAsync(user);
            
            // ✅ Crear usuario automáticamente en PharmDB con las 3 sucursales
            bool pharmUserCreated = false;
            try
            {
                Console.WriteLine($"[DEBUG] Iniciando creación de usuario en PharmDB: {user.Username} (ID: {user.Id})");
                pharmUserCreated = await _pharmApiService.CreateUserInPharmApiAsync(user.Id, user.Username, user.Email);
                Console.WriteLine($"[DEBUG] Resultado creación PharmDB: {pharmUserCreated}");
            }
            catch (Exception ex)
            {
                // Log el error pero no fallar el registro
                Console.WriteLine($"[ERROR] Error creando usuario en PharmDB: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                pharmUserCreated = false;
            }
            
            return Ok(new { 
                message = "Usuario registrado exitosamente",
                sucursalesAsignadas = pharmUserCreated ? 
                    "Se asignaron automáticamente las 3 sucursales de ejemplo (Buenos Aires, Mendoza, Córdoba)" :
                    "ADVERTENCIA: Error al asignar sucursales automáticamente",
                pharmDbSync = pharmUserCreated ? "OK" : "ERROR"
            });
        }


        [HttpPost("test-pharm-connection")]
        public async Task<IActionResult> TestPharmConnection()
        {
            try
            {
                var testResult = await _pharmApiService.CreateUserInPharmApiAsync(999, "test_connection", "test@test.com");
                return Ok(new { 
                    connectionTest = testResult ? "SUCCESS" : "FAILED",
                    message = testResult ? "Conexión a PharmDB exitosa" : "Error en conexión a PharmDB",
                    testUser = "test_connection (ID: 999)"
                });
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    connectionTest = "ERROR",
                    message = $"Error en test de conexión: {ex.Message}",
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResponse = await _userService.LoginAsync(loginDto.Username, loginDto.Password);
            
            if (loginResponse.Success)
            {
                return Ok(loginResponse);
            }
            else
            {
                return Unauthorized(loginResponse);
            }
        }


        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

           
            var userDto = new UserDetailDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Status = user.IsActive ? "Active" : "Inactive",
                DeletedAt = user.DeletedAt
            };

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            await _userService.SoftDeleteAsync(id);
            return Ok(new { message = "Usuario eliminado exitosamente" });
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            
            var user = await _userService.GetByIdIncludeDeletedAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            if (user.IsActive)
            {
                return BadRequest(new { message = "El usuario ya está activo" });
            }

            await _userService.RestoreAsync(id);
            return Ok(new { message = "Usuario restaurado exitosamente" });
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            // Obtener información del usuario desde el token JWT
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new
            {
                message = "Acceso autorizado",
                user = new
                {
                    id = userId,
                    username = username,
                    email = email
                },
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
    }
}