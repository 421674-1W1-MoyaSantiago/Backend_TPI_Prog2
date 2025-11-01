using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class TiposDocumento
    {
        public int CodTipoDocumento { get; set; }

        public string Tipo { get; set; } = null!;

        public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

        public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}
