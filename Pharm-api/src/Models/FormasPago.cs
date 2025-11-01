using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class FormasPago
    {
        public int CodFormaPago { get; set; }

        public string Metodo { get; set; } = null!;

        public virtual ICollection<FacturasVentum> FacturasVenta { get; set; } = new List<FacturasVentum>();
    }
}
