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

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<object>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(new {
                    success = true,
                    totalUsers = users.Count(),
                    users = users.Select(u => new {
                        id = u.Id,
                        username = u.Username,
                        email = u.Email,
                        isActive = u.IsActive,
                        createdAt = u.CreatedAt,
                        updatedAt = u.UpdatedAt,
                        status = u.IsActive ? "Active" : "Inactive"
                    }),
                    message = $"Se encontraron {users.Count()} usuarios en AuthDB"
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

        [HttpGet("all/simple")]
        public async Task<ActionResult<object>> GetAllUsersSimple()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(new {
                    totalUsers = users.Count(),
                    users = users.Select(u => new {
                        id = u.Id,
                        username = u.Username,
                        email = u.Email,
                        isActive = u.IsActive
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    success = false,
                    message = $"Error al obtener usuarios: {ex.Message}"
                });
            }
        }

        [HttpPost("sync/{userId}")]
        [Authorize]
        public async Task<ActionResult<object>> SyncUserToPharm(int userId)
        {
            try
            {
                var user = await _userService.GetByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new {
                        success = false,
                        message = "Usuario no encontrado en Auth-api"
                    });
                }

                var syncSuccess = await _pharmApiService.CreateUserInPharmApiAsync(user.Id, user.Username, user.Email);

                return Ok(new {
                    success = syncSuccess,
                    message = syncSuccess 
                        ? $"Usuario {user.Username} sincronizado exitosamente en Pharm-api"
                        : $"Error al sincronizar usuario {user.Username} en Pharm-api",
                    user = new {
                        id = user.Id,
                        username = user.Username,
                        email = user.Email
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    success = false,
                    message = $"Error al sincronizar usuario: {ex.Message}"
                });
            }
        }

        [HttpPost("sync-all")]
        [Authorize]
        public async Task<ActionResult<object>> SyncAllUsersToPharm()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var results = new List<object>();
                int successCount = 0;
                int failureCount = 0;

                foreach (var user in users)
                {
                    try
                    {
                        var syncSuccess = await _pharmApiService.CreateUserInPharmApiAsync(user.Id, user.Username, user.Email);
                        
                        results.Add(new {
                            userId = user.Id,
                            username = user.Username,
                            success = syncSuccess,
                            message = syncSuccess ? "Sincronizado" : "Error al sincronizar"
                        });

                        if (syncSuccess) successCount++;
                        else failureCount++;
                    }
                    catch (Exception ex)
                    {
                        results.Add(new {
                            userId = user.Id,
                            username = user.Username,
                            success = false,
                            message = $"Exception: {ex.Message}"
                        });
                        failureCount++;
                    }
                }

                return Ok(new {
                    success = true,
                    totalUsers = users.Count(),
                    syncResults = new {
                        successful = successCount,
                        failed = failureCount
                    },
                    details = results,
                    message = $"Proceso completado: {successCount} exitosos, {failureCount} fallidos"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {
                    success = false,
                    message = $"Error al sincronizar usuarios: {ex.Message}"
                });
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
    }
}