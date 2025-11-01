using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class TiposMedicamento
    {
    public int CodTipoMedicamento { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}
