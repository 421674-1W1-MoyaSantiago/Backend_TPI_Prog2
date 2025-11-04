using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Auth_api.PingDTOs;

namespace Auth_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public PingController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Ping simple a la API
    /// </summary>
    [HttpGet]
    public IActionResult Ping()
    {
        var response = new PingResponseDto
        {
            Message = "Pong! API funcionando",
            Timestamp = DateTime.UtcNow,
            Server = Environment.MachineName
        };
        
        return Ok(response);
    }

    /// <summary>
    /// Ping a la base de datos para verificar conectividad
    /// </summary>
    [HttpGet("database")]
    public async Task<IActionResult> PingDatabase()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                return BadRequest(new { Message = "Connection string no configurado" });
            }

            // Prueba básica de conexión
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            
            // Query simple
            using var command = new SqlCommand("SELECT 1", connection);
            var result = await command.ExecuteScalarAsync();
            
            return Ok(new 
            {
                Message = "Conexión exitosa a SQL Server",
                Result = result,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (SqlException sqlEx)
        {
            return Problem($"Error SQL: {sqlEx.Message}", statusCode: 500);
        }
        catch (Exception ex)
        {
            return Problem($"Error general: {ex.Message} | StackTrace: {ex.StackTrace}", statusCode: 500);
        }
    }

    private static string MaskConnectionString(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            return "N/A";
            
        try
        {
            return connectionString.Replace("Root123!", "***");
        }
        catch (Exception)
        {
            return "Error masking connection string";
        }
    }
}