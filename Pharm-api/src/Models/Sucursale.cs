using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Sucursale
    {
        public int CodSucursal { get; set; }

        public string NomSucursal { get; set; } = null!;

        public string? NroTel { get; set; }

        public string? Calle { get; set; }

        public int? Altura { get; set; }

        public string? Email { get; set; }

        public DateTime? HorarioApertura { get; set; }

        public DateTime? HorarioCierre { get; set; }

        public int CodLocalidad { get; set; }

        public virtual Localidade CodLocalidadNavigation { get; set; } = null!;

        public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

        public virtual ICollection<FacturasVentum> FacturasVenta { get; set; } = new List<FacturasVentum>();

        public virtual ICollection<Grupsucursale> Grupsucursales { get; set; } = new List<Grupsucursale>();
    }
}
