using System.ComponentModel.DataAnnotations;

namespace Pharm_api.DTOs
{
    public class MedicamentoDto
    {
        public int CodMedicamento { get; set; }
        public string? CodBarra { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool RequiereReceta { get; set; }
        public bool VentaLibre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int? Dosis { get; set; }
        public string? Posologia { get; set; }
        
        // Información del Lote
        public int CodLoteMedicamento { get; set; }
        public string LoteDescripcion { get; set; } = string.Empty;
        public int StockDisponible { get; set; }
        public DateTime FechaVencimiento { get; set; }
        
        // Información del Laboratorio
        public int CodLaboratorio { get; set; }
        public string LaboratorioDescripcion { get; set; } = string.Empty;
        
        // Información del Tipo de Presentación
        public int CodTipoPresentacion { get; set; }
        public string TipoPresentacionDescripcion { get; set; } = string.Empty;
        
        // Información de la Unidad de Medida
        public int CodUnidadMedida { get; set; }
        public string UnidadMedidaDescripcion { get; set; } = string.Empty;
        
        // Información del Tipo de Medicamento
        public int CodTipoMedicamento { get; set; }
        public string TipoMedicamentoDescripcion { get; set; } = string.Empty;
    }

    public class CreateMedicamentoDto
    {
        [StringLength(50, ErrorMessage = "El código de barra no puede exceder 50 caracteres")]
        public string? CodBarra { get; set; }
        
        [Required(ErrorMessage = "La descripción del medicamento es requerida")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
        
        public bool RequiereReceta { get; set; }
        
        public bool VentaLibre { get; set; }
        
        [Required(ErrorMessage = "El precio unitario es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "La dosis debe ser mayor a 0")]
        public int? Dosis { get; set; }
        
        [StringLength(500, ErrorMessage = "La posología no puede exceder 500 caracteres")]
        public string? Posologia { get; set; }
        
        [Required(ErrorMessage = "El lote de medicamento es requerido")]
        public int CodLoteMedicamento { get; set; }
        
        [Required(ErrorMessage = "El laboratorio es requerido")]
        public int CodLaboratorio { get; set; }
        
        [Required(ErrorMessage = "El tipo de presentación es requerido")]
        public int CodTipoPresentacion { get; set; }
        
        [Required(ErrorMessage = "La unidad de medida es requerida")]
        public int CodUnidadMedida { get; set; }
        
        [Required(ErrorMessage = "El tipo de medicamento es requerido")]
        public int CodTipoMedicamento { get; set; }
    }

    public class UpdateMedicamentoDto
    {
        [StringLength(50, ErrorMessage = "El código de barra no puede exceder 50 caracteres")]
        public string? CodBarra { get; set; }
        
        [Required(ErrorMessage = "La descripción del medicamento es requerida")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
        
        public bool RequiereReceta { get; set; }
        
        public bool VentaLibre { get; set; }
        
        [Required(ErrorMessage = "El precio unitario es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "La dosis debe ser mayor a 0")]
        public int? Dosis { get; set; }
        
        [StringLength(500, ErrorMessage = "La posología no puede exceder 500 caracteres")]
        public string? Posologia { get; set; }
        
        [Required(ErrorMessage = "El lote de medicamento es requerido")]
        public int CodLoteMedicamento { get; set; }
        
        [Required(ErrorMessage = "El laboratorio es requerido")]
        public int CodLaboratorio { get; set; }
        
        [Required(ErrorMessage = "El tipo de presentación es requerido")]
        public int CodTipoPresentacion { get; set; }
        
        [Required(ErrorMessage = "La unidad de medida es requerida")]
        public int CodUnidadMedida { get; set; }
        
        [Required(ErrorMessage = "El tipo de medicamento es requerido")]
        public int CodTipoMedicamento { get; set; }
    }
}