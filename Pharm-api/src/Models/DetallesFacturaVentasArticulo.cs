namespace Pharm_api.Models
{
    public partial class DetallesFacturaVentasArticulo
    {
        public int cod_DetFacVentaA { get; set; }
        public int codFacturaVenta { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public int codArticulo { get; set; }
        public bool Anulada { get; set; } = false;
        public virtual FacturasVentum FacturaVenta { get; set; } = null!;
        public virtual Articulo Articulo { get; set; } = null!;
    }
}
