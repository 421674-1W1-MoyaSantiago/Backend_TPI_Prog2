namespace Pharm_api.DTOs
{
    public class RecetasObraSocialEstadoDto
    {
        public string? NombreMedico { get; set; }
        public string? NroMatricula { get; set; }
        public string? FechaReceta { get; set; } // en el SP está convertida a VARCHAR(103)
        public string? Diagnostico { get; set; }
        public string? TipoReceta { get; set; }
        public string? EstadoReceta { get; set; }
        public string? NombreCliente { get; set; }
        public string? NroDocumentoCliente { get; set; }
        public string? ObraSocial { get; set; }
        public string? EstadoAutorizacion { get; set; }
    }

}
