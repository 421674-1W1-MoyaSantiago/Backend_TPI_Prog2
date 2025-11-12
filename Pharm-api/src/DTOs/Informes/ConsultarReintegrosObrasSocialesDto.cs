namespace Pharm_api.DTOs
{
    public class ConsultarReintegrosObrasSocialesDto
    {
        public int CodigoReintegro { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime? FechaReembolso { get; set; }
        public string? EstadoReintegro { get; set; } = string.Empty;
        public int DiasTranscurridos { get; set; }
        public string? ObraSocial { get; set; } = string.Empty;
        public string? Cliente { get; set; } = string.Empty;
        public string? DniCliente { get; set; } = string.Empty;
        public string? Medicamento { get; set; } = string.Empty;
        public string? CodigoDeBarras { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? PorcentajeDescuento { get; set; }
        public string? TipoDescuento { get; set; } = string.Empty;
        public decimal? SubtotalVenta { get; set; }
        public decimal? MontoDescuento { get; set; }
        public decimal? TotalAReintegrar { get; set; }
        public int NroFactura { get; set; }
        public DateTime FechaVenta { get; set; }
        public string? FormaDePago { get; set; } = string.Empty;
        public string? Sucursal { get; set; } = string.Empty;
        public string? Empleado { get; set; } = string.Empty;
    }
}
