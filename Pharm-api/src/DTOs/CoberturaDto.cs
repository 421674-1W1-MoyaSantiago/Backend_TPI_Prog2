namespace Pharm_api.DTOs
{
    public class CoberturaDto
    {
    public int CodCobertura { get; set; }
    public int IdCliente { get; set; }
    public string? NombreCliente { get; set; }
    public string? NombreObraSocial { get; set; }
    public string? NombreDescuento { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    }
}
