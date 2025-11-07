using System.Text;
using System.Text.Json;

namespace Auth_api.Services
{
    public class PharmApiService : IPharmApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PharmApiService> _logger;

        public PharmApiService(HttpClient httpClient, ILogger<PharmApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> CreateUserInPharmApiAsync(int userId, string username, string email)
        {
            try
            {
                _logger.LogInformation($"[HTTP] Notificando a Pharm-api sobre nuevo usuario: {username} (ID: {userId})");

                var payload = new
                {
                    UserId = userId,
                    Username = username,
                    Email = email
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation($"[HTTP] Enviando POST a /api/sync/user-from-auth");
                _logger.LogInformation($"[HTTP] Payload: {json}");

                var response = await _httpClient.PostAsync("/api/sync/user-from-auth", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"[HTTP] SUCCESS: Usuario {username} creado en Pharm-api");
                    _logger.LogInformation($"[HTTP] Response: {responseContent}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"[HTTP] ERROR: Pharm-api respondió {response.StatusCode}");
                    _logger.LogWarning($"[HTTP] Error response: {errorContent}");
                    return false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, $"[HTTP] Error de conexión con Pharm-api: {httpEx.Message}");
                return false;
            }
            catch (TaskCanceledException timeoutEx)
            {
                _logger.LogError(timeoutEx, $"[HTTP] Timeout al conectar con Pharm-api: {timeoutEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[HTTP] Error inesperado al notificar Pharm-api: {ex.Message}");
                return false;
            }
        }
    }
}
        }
    }
}