using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class TiposEmpleado
    {
    public int CodTipoEmpleado { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}
