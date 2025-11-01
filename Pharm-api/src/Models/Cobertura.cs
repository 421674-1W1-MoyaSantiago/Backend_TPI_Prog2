using System;
using System.Collections.Generic;

namespace Pharm_api.Models;

public partial class Cobertura
{
    public int CodCobertura { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public int CodLocalidad { get; set; }

    public int CodCliente { get; set; }

    public int CodObraSocial { get; set; }

    public int CodDescuento { get; set; }

    public virtual Cliente CodClienteNavigation { get; set; } = null!;

    public virtual Descuento CodDescuentoNavigation { get; set; } = null!;

    public virtual Localidade CodLocalidadNavigation { get; set; } = null!;

    public virtual ObrasSociale CodObraSocialNavigation { get; set; } = null!;

    public virtual ICollection<DetallesFacturaVentasMedicamento> DetallesFacturaVentasMedicamentos { get; set; } = new List<DetallesFacturaVentasMedicamento>();
}