using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Empleado
    {
        public int CodEmpleado { get; set; }

        public string NomEmpleado { get; set; } = null!;

        public string ApeEmpleado { get; set; } = null!;

        public string? NroTel { get; set; }

        public string? Calle { get; set; }

        public int? Altura { get; set; }

        public string? Email { get; set; }

        public DateTime FechaIngreso { get; set; }

        public int CodTipoEmpleado { get; set; }

        public int CodTipoDocumento { get; set; }

        public int CodSucursal { get; set; }

        public virtual Sucursale CodSucursalNavigation { get; set; } = null!;

        public virtual TiposDocumento CodTipoDocumentoNavigation { get; set; } = null!;

        public virtual TiposEmpleado CodTipoEmpleadoNavigation { get; set; } = null!;

        public virtual ICollection<FacturasVentum> FacturasVenta { get; set; } = new List<FacturasVentum>();
    }
}
