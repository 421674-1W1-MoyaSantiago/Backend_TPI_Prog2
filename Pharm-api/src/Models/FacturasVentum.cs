using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class FacturasVentum
    {
        public int CodFacturaVenta { get; set; }

        public DateTime Fecha { get; set; }

        public int CodEmpleado { get; set; }

        public int CodCliente { get; set; }

        public int CodSucursal { get; set; }

        public int CodFormaPago { get; set; }

        public decimal? Total { get; set; }

        public virtual Cliente CodClienteNavigation { get; set; } = null!;

        public virtual Empleado CodEmpleadoNavigation { get; set; } = null!;

        public virtual FormasPago CodFormaPagoNavigation { get; set; } = null!;

        public virtual Sucursale CodSucursalNavigation { get; set; } = null!;
    }
}
