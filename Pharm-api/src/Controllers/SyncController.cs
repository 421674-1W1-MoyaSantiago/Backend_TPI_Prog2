using Microsoft.AspNetCore.Mvc;
using Pharm_api.DTOs;
using Pharm_api.Services;

namespace Pharm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly IUserSyncService _userSyncService;
        private readonly ILogger<SyncController> _logger;

        public SyncController(IUserSyncService userSyncService, ILogger<SyncController> logger)
        {
            _userSyncService = userSyncService;
            _logger = logger;
        }

        [HttpPost("user-from-auth")]
        public async Task<ActionResult<UserSyncResponseDto>> CreateUserFromAuth([FromBody] CreateUserFromAuthDto dto)
        {
            try
            {
                _logger.LogInformation($"Recibida notificación de Auth-api para usuario: {dto.Username} (ID: {dto.UserId})");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userSyncService.CreateUserFromAuthAsync(dto);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(500, result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en endpoint de sincronización para usuario {dto.Username}");
                return StatusCode(500, new {
                    success = false,
                    message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        [HttpGet("user-exists/{userId}")]
        public async Task<ActionResult<object>> CheckUserExists(int userId)
        {
            try
            {
                var exists = await _userSyncService.UserExistsAsync(userId);
                return Ok(new {
                    userId = userId,
                    exists = exists,
                    message = exists ? "Usuario existe en PharmDB" : "Usuario NO existe en PharmDB"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verificando existencia del usuario {userId}");
                return StatusCode(500, new {
                    success = false,
                    message = $"Error verificando usuario: {ex.Message}"
                });
            }
        }
    }
}