using System.ComponentModel.DataAnnotations;

namespace Pharm_api.DTOs
{
    public class EmpleadoDto
    {
        public int CodEmpleado { get; set; }
        public string NomEmpleado { get; set; } = string.Empty;
        public string ApeEmpleado { get; set; } = string.Empty;
        public string? NroTel { get; set; }
        public string? Calle { get; set; }
        public int? Altura { get; set; }
        public string? Email { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int CodTipoEmpleado { get; set; }
        public string TipoEmpleado { get; set; } = string.Empty;
        public int CodTipoDocumento { get; set; }
        public string TipoDocumento { get; set; } = string.Empty;
        public int CodSucursal { get; set; }
        public string NomSucursal { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class CreateEmpleadoDto
    {
        [Required(ErrorMessage = "El nombre del empleado es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NomEmpleado { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El apellido del empleado es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string ApeEmpleado { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string? NroTel { get; set; }
        
        [StringLength(200, ErrorMessage = "La calle no puede exceder 200 caracteres")]
        public string? Calle { get; set; }
        
        [Range(1, 99999, ErrorMessage = "La altura debe ser un número válido")]
        public int? Altura { get; set; }
        
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "La fecha de ingreso es requerida")]
        public DateTime FechaIngreso { get; set; }
        
        [Required(ErrorMessage = "El tipo de empleado es requerido")]
        public int CodTipoEmpleado { get; set; }
        
        [Required(ErrorMessage = "El tipo de documento es requerido")]
        public int CodTipoDocumento { get; set; }
        
        [Required(ErrorMessage = "La sucursal es requerida")]
        public int CodSucursal { get; set; }
    }

    public class UpdateEmpleadoDto
    {
        [Required(ErrorMessage = "El nombre del empleado es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string NomEmpleado { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El apellido del empleado es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string ApeEmpleado { get; set; } = string.Empty;
        
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string? NroTel { get; set; }
        
        [StringLength(200, ErrorMessage = "La calle no puede exceder 200 caracteres")]
        public string? Calle { get; set; }
        
        [Range(1, 99999, ErrorMessage = "La altura debe ser un número válido")]
        public int? Altura { get; set; }
        
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "El tipo de empleado es requerido")]
        public int CodTipoEmpleado { get; set; }
        
        [Required(ErrorMessage = "El tipo de documento es requerido")]
        public int CodTipoDocumento { get; set; }
        
        [Required(ErrorMessage = "La sucursal es requerida")]
        public int CodSucursal { get; set; }
    }
}