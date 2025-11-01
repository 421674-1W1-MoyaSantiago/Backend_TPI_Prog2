namespace Pharm_api.DTOs;

public class CreateUsuarioDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class AsignarSucursalesDto
{
    public List<int> Sucursales { get; set; } = new List<int>();
}

public class UsuarioSucursalDto
{
    public int CodSucursal { get; set; }
    public string NomSucursal { get; set; } = string.Empty;
    public DateTime FechaAsignacion { get; set; }
    public bool Activo { get; set; }
}

    public class UsuarioDto
    {
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    }