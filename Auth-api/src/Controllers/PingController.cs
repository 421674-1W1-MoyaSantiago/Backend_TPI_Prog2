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
                return BadRequest(new ErrorResponseDto { Message = "Connection string no configurado" });
            }

            // Primero probar conexión con master
            var masterConnectionString = connectionString.Replace("Database=AuthDB", "Database=master");
            
            using var masterConnection = new SqlConnection(masterConnectionString);
            await masterConnection.OpenAsync();
            
            // Verificar si AuthDB existe
            using var checkDbCommand = new SqlCommand("SELECT COUNT(*) FROM sys.databases WHERE name = 'AuthDB'", masterConnection);
            var dbCount = await checkDbCommand.ExecuteScalarAsync();
            var dbExists = dbCount != null && (int)dbCount > 0;
            
            if (!dbExists)
            {
                return Ok(new DatabasePingResponseDto
                {
                    Message = "Conexión exitosa pero AuthDB no existe. Use POST /api/ping/setup para crearla.",
                    Result = 0,
                    ServerTime = DateTime.UtcNow,
                    ConnectionString = MaskConnectionString(masterConnectionString)
                });
            }
            
            // Ahora probar con AuthDB
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            
            // Ejecutar un query simple
            using var command = new SqlCommand("SELECT 1 as Result, GETDATE() as ServerTime", connection);
            using var reader = await command.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                var result = reader.GetInt32(0);
                var serverTime = reader.GetDateTime(1);
                
                var response = new DatabasePingResponseDto
                {
                    Message = "Pong! Base de datos AuthDB conectada exitosamente",
                    Result = result,
                    ServerTime = serverTime,
                    ConnectionString = MaskConnectionString(connectionString)
                };
                
                return Ok(response);
            }
            
            return Problem("No se pudo obtener respuesta de la base de datos");
        }
        catch (SqlException sqlEx)
        {
            return Problem($"Error SQL: {sqlEx.Message}", statusCode: 500);
        }
        catch (Exception ex)
        {
            return Problem($"Error conectando a la base de datos: {ex.Message}", statusCode: 500);
        }
    }

    private static string MaskConnectionString(string connectionString)
    {
        return connectionString.Replace("Root123!", "***");
    }
}