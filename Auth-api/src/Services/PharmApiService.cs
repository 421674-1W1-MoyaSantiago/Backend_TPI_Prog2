using Microsoft.Data.SqlClient;

namespace Auth_api.Services
{
    public class PharmApiService : IPharmApiService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PharmApiService> _logger;

        public PharmApiService(IConfiguration configuration, ILogger<PharmApiService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> CreateUserInPharmApiAsync(int userId, string username, string email)
        {
            try
            {
                // Obtener la connection string de PharmDB
                var pharmConnectionString = _configuration.GetConnectionString("PharmDatabase");
                
                if (string.IsNullOrEmpty(pharmConnectionString))
                {
                    _logger.LogWarning("PharmDatabase connection string no está configurada");
                    return false;
                }

                using var connection = new SqlConnection(pharmConnectionString);
                await connection.OpenAsync();

                using var command = new SqlCommand("sp_UpdateUserFromToken", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);

                await command.ExecuteNonQueryAsync();
                
                _logger.LogInformation($"Usuario {username} creado exitosamente en PharmDB con sucursales asignadas");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creando usuario {username} en PharmDB: {ex.Message}");
                return false;
            }
        }
    }
}