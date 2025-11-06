using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class StockMedicamento
    {
        public int CodStockMedicamento { get; set; }

        public int Cantidad { get; set; }

        public int CodSucursal { get; set; }

        public int CodMedicamento { get; set; }

        public virtual Sucursale CodSucursalNavigation { get; set; } = null!;

        public virtual Medicamento CodMedicamentoNavigation { get; set; } = null!;
    }
}