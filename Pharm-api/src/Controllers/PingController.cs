using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Pharm_api.PingDTOs;

namespace Pharm_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public PingController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

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

            var masterConnectionString = connectionString.Replace("Database=PharmDB", "Database=master");

            using var masterConnection = new SqlConnection(masterConnectionString);
            await masterConnection.OpenAsync();

           
            using var checkDbCommand = new SqlCommand("SELECT COUNT(*) FROM sys.databases WHERE name = 'PharmDB'", masterConnection);
            var dbCount = await checkDbCommand.ExecuteScalarAsync();
            var dbExists = dbCount != null && (int)dbCount > 0;

            if (!dbExists)
            {
                return Ok(new DatabasePingResponseDto
                {
                    Message = "Conexión exitosa pero PharmDB no existe. Use POST /api/ping/setup para crearla.",
                    Result = 0,
                    ServerTime = DateTime.UtcNow,
                    ConnectionString = MaskConnectionString(masterConnectionString)
                });
            }

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT 1 as Result, GETDATE() as ServerTime", connection);
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var result = reader.GetInt32(0);
                var serverTime = reader.GetDateTime(1);

                return Ok(new DatabasePingResponseDto
                {
                    Message = "Pong! Base de datos PharmDB conectada exitosamente",
                    Result = result,
                    ServerTime = serverTime,
                    ConnectionString = MaskConnectionString(connectionString)
                });
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