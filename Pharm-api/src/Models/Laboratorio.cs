using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Laboratorio
    {
        public int CodLaboratorio { get; set; }

        public string Descripcion { get; set; } = null!;

        public int CodProveedor { get; set; }

        public virtual Proveedore CodProveedorNavigation { get; set; } = null!;

        public virtual ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}
