namespace Auth_api.PingDTOs{
    public class PingResponseDto
    {
        public string Message { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string Server { get; set; } = null!;
    }

    public class DatabasePingResponseDto
    {
        public string Message { get; set; } = null!;
        public int Result { get; set; }
        public DateTime ServerTime { get; set; }
        public string ConnectionString { get; set; } = null!;
    }

    public class SetupResponseDto
    {
        public string Message { get; set; } = null!;
        public string Database { get; set; } = null!;
        public string Table { get; set; } = null!;
    }

    public class ErrorResponseDto
    {
        public string Message { get; set; } = null!;
    }

}

