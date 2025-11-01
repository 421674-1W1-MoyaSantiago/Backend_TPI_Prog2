using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class ObrasSociale
    {
        public int CodObraSocial { get; set; }

        public string RazonSocial { get; set; } = null!;

        public string? NroTel { get; set; }

        public string? Email { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    }
}
