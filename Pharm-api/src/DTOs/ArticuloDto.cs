namespace Pharm_api.DTOs
{
    public class ArticuloDto
    {
        public int CodArticulo { get; set; }
        public string? CodBarra { get; set; }
        public string Descripcion { get; set; } = null!;
        public decimal PrecioUnitario { get; set; }
    }
}
