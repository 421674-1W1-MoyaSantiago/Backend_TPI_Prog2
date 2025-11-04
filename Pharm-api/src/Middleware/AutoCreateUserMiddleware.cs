using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;

namespace Pharm_api.Middleware
{
    public class AutoCreateUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AutoCreateUserMiddleware> _logger;

        public AutoCreateUserMiddleware(RequestDelegate next, ILogger<AutoCreateUserMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, PharmDbContext dbContext)
        {
            // Solo procesar si el usuario está autenticado
            if (context.User.Identity?.IsAuthenticated == true)
            {
                try
                {
                    // Extraer información del token
                    var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var username = context.User.FindFirst(ClaimTypes.Name)?.Value;
                    var email = context.User.FindFirst(ClaimTypes.Email)?.Value;

                    if (!string.IsNullOrEmpty(userIdClaim) && 
                        !string.IsNullOrEmpty(username) && 
                        !string.IsNullOrEmpty(email) &&
                        int.TryParse(userIdClaim, out int userId))
                    {
                        // Llamar al stored procedure para crear/actualizar usuario
                        await UpdateUserFromToken(dbContext, userId, username, email);
                        
                        _logger.LogInformation($"Usuario {username} sincronizado automáticamente");
                    }
                }
                catch (Exception ex)
                {
                    // Log del error pero no interrumpir el request
                    _logger.LogWarning($"Error en sincronización de usuario: {ex.Message}");
                }
            }

            // Continuar con el siguiente middleware
            await _next(context);
        }

        private async Task UpdateUserFromToken(PharmDbContext dbContext, int userId, string username, string email)
        {
            try
            {
                // Ejecutar el stored procedure
                var userIdParam = new SqlParameter("@UserId", userId);
                var usernameParam = new SqlParameter("@Username", username);
                var emailParam = new SqlParameter("@Email", email);

                await dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC sp_UpdateUserFromToken @UserId, @Username, @Email",
                    userIdParam, usernameParam, emailParam);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ejecutando sp_UpdateUserFromToken: {ex.Message}");
            }
        }
    }
}