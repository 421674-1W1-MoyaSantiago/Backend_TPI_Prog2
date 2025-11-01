using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class UnidadesMedidum
    {
    public int CodUnidadMedida { get; set; }

    public string UnidadMedida { get; set; } = null!;

    public virtual ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}
