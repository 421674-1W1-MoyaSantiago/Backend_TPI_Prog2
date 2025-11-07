namespace Pharm_api.DTOs;

public class CreateUsuarioDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? CodUsuario { get; set; } // Para sincronización con Auth-api
}

public class CreateUserFromAuthDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class AsignarSucursalesDto
{
    public List<int> Sucursales { get; set; } = new List<int>();
}

public class UserSyncResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? UsuarioId { get; set; }
    public string? Username { get; set; }
    public List<string> SucursalesAsignadas { get; set; } = new List<string>();
    public string? Error { get; set; }
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