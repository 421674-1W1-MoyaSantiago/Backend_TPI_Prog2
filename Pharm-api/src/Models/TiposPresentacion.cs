using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class TiposPresentacion
    {
    public int CodTipoPresentacion { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}
