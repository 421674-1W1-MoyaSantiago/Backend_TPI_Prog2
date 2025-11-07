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
                
                _logger.LogInformation($"[DEBUG] Iniciando creación usuario: {username} (ID: {userId})");
                _logger.LogInformation($"[DEBUG] Email: {email}");
                
                if (string.IsNullOrEmpty(pharmConnectionString))
                {
                    _logger.LogWarning("[ERROR] PharmDatabase connection string no está configurada");
                    Console.WriteLine("[ERROR] PharmDatabase connection string no está configurada");
                    return false;
                }
                
                _logger.LogInformation($"[DEBUG] Connection string encontrada: {pharmConnectionString.Substring(0, 50)}...");

                using var connection = new SqlConnection(pharmConnectionString);
                
                _logger.LogInformation("[DEBUG] Abriendo conexión a PharmDB...");
                await connection.OpenAsync();
                _logger.LogInformation("[DEBUG] Conexión abierta exitosamente");

                using var command = new SqlCommand("sp_UpdateUserFromToken", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);

                _logger.LogInformation($"[DEBUG] Ejecutando stored procedure con parámetros: UserId={userId}, Username={username}, Email={email}");
                
                await command.ExecuteNonQueryAsync();
                
                _logger.LogInformation($"[SUCCESS] Usuario {username} creado exitosamente en PharmDB con sucursales asignadas");
                Console.WriteLine($"[SUCCESS] Usuario {username} creado exitosamente en PharmDB");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ERROR] Error creando usuario {username} en PharmDB: {ex.Message}");
                Console.WriteLine($"[ERROR] Error creando usuario {username} en PharmDB: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}