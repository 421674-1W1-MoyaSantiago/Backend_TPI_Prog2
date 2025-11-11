namespace Pharm_api.DTOs
{
    // DTO base para todos los detalles
    public abstract class DetalleFacturaBaseDto
    {
    public int CodigoDetalle { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;
    public abstract string TipoDetalle { get; }

    // Propiedades virtuales para nombre, siempre presentes en la serialización
    public virtual string? NombreMedicamento { get; set; }
    public virtual string? NombreArticulo { get; set; }
    }

    // DTO específico para detalles de medicamentos
    public class DetalleMedicamentoFacturaDto : DetalleFacturaBaseDto
    {
    public override string TipoDetalle => "Medicamento";
    public int CodMedicamento { get; set; }
    public override string? NombreMedicamento { get; set; } = string.Empty;
    public override string? NombreArticulo { get; set; } = null;
    public string? Concentracion { get; set; }
    public string? Presentacion { get; set; }
    public int? CodCobertura { get; set; }
    public string? NombreCobertura { get; set; }
    }

    // DTO específico para detalles de artículos
    public class DetalleArticuloFacturaDto : DetalleFacturaBaseDto
    {
    public override string TipoDetalle => "Articulo";
    public int CodArticulo { get; set; }
    public override string? NombreMedicamento { get; set; } = null;
    public override string? NombreArticulo { get; set; } = string.Empty;
    public string? Marca { get; set; }
    }

    // DTOs antiguos para compatibilidad (se pueden eliminar después)
    public class DetalleMedicamentoDto
    {
    public int CodDetFacVentaM { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int? CodCobertura { get; set; }
    public string? NombreCobertura { get; set; }
    public int CodMedicamento { get; set; }
    public string NombreMedicamento { get; set; } = string.Empty;
    public string? Laboratorio { get; set; }
    public string? Lote { get; set; }
    public string? Tipo { get; set; }
    public string? Presentacion { get; set; }
    public string? UnidadMedida { get; set; }
    public string? Concentracion { get; set; }
    }

    public class DetalleArticuloDto
    {
        public int CodDetFacVentaA { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int CodArticulo { get; set; }
        public string NombreArticulo { get; set; } = string.Empty;
        public string? Marca { get; set; }
    }

    public class FacturaVentaDto
    {
        public int CodFacturaVenta { get; set; }
        public DateTime Fecha { get; set; }
        public int CodEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public int CodCliente { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public int CodSucursal { get; set; }
        public string NombreSucursal { get; set; } = string.Empty;
        public int CodFormaPago { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public decimal Total { get; set; }
        
        // Nueva estructura con DTOs específicos
        public List<DetalleFacturaBaseDto> Detalles { get; set; } = new();
    }

    public class CreateFacturaVentaDto
    {
        public int CodEmpleado { get; set; }
        public int CodCliente { get; set; }
        public int CodSucursal { get; set; }
        public int CodFormaPago { get; set; }
        public IEnumerable<CrearDetalleArticuloDto>? DetalleArticulos { get; set; }
        public IEnumerable<CrearDetalleMedicamentoDto>? DetalleMedicamentos { get; set; }
    }

    public class CrearDetalleMedicamentoDto
    {
        public int CodMedicamento { get; set; }
        public int? CodCobertura { get; set; }
        public int Cantidad { get; set; }
    }

    public class CrearDetalleArticuloDto
    {
        public int CodArticulo { get; set; }
        public int Cantidad { get; set; }
    }

    public class FacturaVentaQueryDto
    {
        public int? CodSucursal { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? CodEmpleado { get; set; }
        public int? CodCliente { get; set; }
    }
}