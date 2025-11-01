using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Usuario
    {
    public int CodUsuario { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Grupsucursale> Grupsucursales { get; set; } = new List<Grupsucursale>();
    }
}
