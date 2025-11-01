using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Localidade
    {
    public int CodLocalidad { get; set; }

    public string NomLocalidad { get; set; } = null!;

    public int CodProvincia { get; set; }

    public virtual Provincia CodProvinciaNavigation { get; set; } = null!;

    public virtual ICollection<Sucursale> Sucursales { get; set; } = new List<Sucursale>();
    }
}
