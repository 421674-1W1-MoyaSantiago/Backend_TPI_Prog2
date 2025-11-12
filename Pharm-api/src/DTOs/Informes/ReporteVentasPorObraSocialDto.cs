namespace Pharm_api.DTOs
{
    public class ReporteVentasPorObraSocialDto
    {
        public string ObraSocial { get; set; }
        public int CantidadVentas { get; set; }
        public int ClientesDistintos { get; set; }
        public decimal TotalRecaudado { get; set; }
        public decimal PromedioVenta { get; set; }
        public decimal PorcentajeTotalRecaudado { get; set; } 
        public int ReintegrosPendientes { get; set; }
    }

}
