namespace Pharm_api.PingDTOs;

public class PingResponseDto
{
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Server { get; set; } = string.Empty;
}

public class DatabasePingResponseDto
{
    public string Message { get; set; } = string.Empty;
    public int Result { get; set; }
    public DateTime ServerTime { get; set; }
    public string ConnectionString { get; set; } = string.Empty;
}

public class ErrorResponseDto
{
    public string Message { get; set; } = string.Empty;
}