namespace Pharm_api.Models
{
    public partial class DetallesFacturaVentasMedicamento
    {
        public int cod_DetFacVentaM { get; set; }
        public int codFacturaVenta { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public int? codCobertura { get; set; }
        public int codMedicamento { get; set; }
        
        public virtual FacturasVentum FacturaVenta { get; set; } = null!;
        public virtual Medicamento Medicamento { get; set; } = null!;
        public virtual Cobertura? Cobertura { get; set; }
    }
}
