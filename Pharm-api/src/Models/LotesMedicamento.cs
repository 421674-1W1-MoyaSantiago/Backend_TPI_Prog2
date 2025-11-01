using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class LotesMedicamento
    {
    public int CodLoteMedicamento { get; set; }

    public DateTime FechaElaboracion { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public int Cantidad { get; set; }

    public virtual ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}
