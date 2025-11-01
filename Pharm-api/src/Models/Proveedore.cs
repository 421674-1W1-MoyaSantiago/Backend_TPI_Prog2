using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Proveedore
    {
    public int CodProveedor { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string? Cuit { get; set; }

    public string? NroTel { get; set; }

    public virtual ICollection<Laboratorio> Laboratorios { get; set; } = new List<Laboratorio>();
    }
}
