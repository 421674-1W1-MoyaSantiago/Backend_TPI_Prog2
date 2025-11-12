namespace Pharm_api.DTOs
{
    public class VentasMedicamentosObraSocialDto
    {
        public DateTime FechaVenta { get; set; }
        public string? Cliente { get; set; }
        public string? Medicamento { get; set; }
        public int? CantidadVendida { get; set; }
        public string? Medico { get; set; }
        public string? Matricula { get; set; }
        public string? ObraSocial { get; set; }
        public decimal? MontoDescuento { get; set; }
        public int? StockActual { get; set; }
        public string? TipoVenta { get; set; }
    }

}
