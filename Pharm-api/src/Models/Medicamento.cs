using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class Medicamento
    {
        public int CodMedicamento { get; set; }

        public string? CodBarra { get; set; }

        public string Descripcion { get; set; } = null!;

        public bool RequiereReceta { get; set; }

        public bool VentaLibre { get; set; }

        public decimal PrecioUnitario { get; set; }

        public int? Dosis { get; set; }

        public string? Posologia { get; set; }

        public int CodLoteMedicamento { get; set; }

        public int CodLaboratorio { get; set; }

        public int CodTipoPresentacion { get; set; }

        public int CodUnidadMedida { get; set; }

        public int CodTipoMedicamento { get; set; }

        public virtual Laboratorio CodLaboratorioNavigation { get; set; } = null!;

        public virtual LotesMedicamento CodLoteMedicamentoNavigation { get; set; } = null!;

        public virtual TiposMedicamento CodTipoMedicamentoNavigation { get; set; } = null!;

        public virtual TiposPresentacion CodTipoPresentacionNavigation { get; set; } = null!;

        public virtual UnidadesMedidum CodUnidadMedidaNavigation { get; set; } = null!;
    }
}
