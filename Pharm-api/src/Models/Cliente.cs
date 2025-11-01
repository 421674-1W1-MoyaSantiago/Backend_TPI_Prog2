using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Cliente
    {
    public int CodCliente { get; set; }

    public string NomCliente { get; set; } = null!;

    public string ApeCliente { get; set; } = null!;

    public string? NroDoc { get; set; }

    public string? NroTel { get; set; }

    public string? Calle { get; set; }

    public int? Altura { get; set; }

    public string? Email { get; set; }

    public int CodTipoDocumento { get; set; }

    public int CodObraSocial { get; set; }

    public virtual ObrasSociale CodObraSocialNavigation { get; set; } = null!;

    public virtual TiposDocumento CodTipoDocumentoNavigation { get; set; } = null!;

    public virtual ICollection<FacturasVentum> FacturasVenta { get; set; } = new List<FacturasVentum>();
    }
}
