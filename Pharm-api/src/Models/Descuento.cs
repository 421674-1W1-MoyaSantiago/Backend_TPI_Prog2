using System;
using System.Collections.Generic;

namespace Pharm_api.Models;

public partial class Descuento
{
    public int CodDescuento { get; set; }

    public DateTime FechaDescuento { get; set; }

    public int CodLocalidad { get; set; }

    public int CodMedicamento { get; set; }

    public decimal PorcentajeDescuento { get; set; }

    public int CodTipoDescuento { get; set; }

    public virtual Localidade CodLocalidadNavigation { get; set; } = null!;

    public virtual Medicamento CodMedicamentoNavigation { get; set; } = null!;

    public virtual TiposDescuento CodTipoDescuentoNavigation { get; set; } = null!;

    public virtual ICollection<Cobertura> Coberturas { get; set; } = new List<Cobertura>();
}