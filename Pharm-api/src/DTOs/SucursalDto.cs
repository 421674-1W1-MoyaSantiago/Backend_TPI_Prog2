namespace Pharm_api.DTOs;

public class SucursalDto
{
    public int CodSucursal { get; set; }
    public string NomSucursal { get; set; } = string.Empty;
    public string? NroTel { get; set; }
    public string? Calle { get; set; }
    public int? Altura { get; set; }
    public string? Email { get; set; }
    public DateTime? HorarioApertura { get; set; }
    public DateTime? HorarioCierre { get; set; }
    public int CodLocalidad { get; set; }
    public string NomLocalidad { get; set; } = string.Empty; // From Localidade.NomLocalidad

    // Propiedades para asignación de sucursales a usuario
    public DateTime? FechaAsignacion { get; set; }
    public bool Activo { get; set; }
}