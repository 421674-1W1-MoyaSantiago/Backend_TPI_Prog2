using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Provincia
    {
        public int CodProvincia { get; set; }

        public string NomProvincia { get; set; } = null!;

        public virtual ICollection<Localidade> Localidades { get; set; } = new List<Localidade>();
    }
}
