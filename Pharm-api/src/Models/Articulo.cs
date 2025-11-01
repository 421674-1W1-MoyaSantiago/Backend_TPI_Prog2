using System;
using System.Collections.Generic;

namespace Pharm_api.Models;

public partial class Articulo
{
    public int CodArticulo { get; set; }

    public string? CodBarra { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal PrecioUnitario { get; set; }

    public int CodProveedor { get; set; }

    public int CodCategoriaArticulo { get; set; }

    public virtual CategoriasArticulo CodCategoriaArticuloNavigation { get; set; } = null!;

    public virtual Proveedore CodProveedorNavigation { get; set; } = null!;

    public virtual ICollection<DetallesFacturaVentasArticulo> DetallesFacturaVentasArticulos { get; set; } = new List<DetallesFacturaVentasArticulo>();
}