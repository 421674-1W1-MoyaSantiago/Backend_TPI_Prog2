using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Grupsucursale
    {
    public int Id { get; set; }

    public int CodUsuario { get; set; }

    public int CodSucursal { get; set; }

    public DateTime FechaAsignacion { get; set; }

    public bool Activo { get; set; }

    public virtual Sucursale CodSucursalNavigation { get; set; } = null!;

    public virtual Usuario CodUsuarioNavigation { get; set; } = null!;
    }
}
