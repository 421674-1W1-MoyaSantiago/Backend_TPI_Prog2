using System;
using System.Collections.Generic;

namespace Pharm_api.Models
{
    public partial class CategoriasArticulo
    {
        public int CodCategoriaArticulo { get; set; }

        public string Categoria { get; set; } = null!;
        public string Descripcion { get; set; }
    }
}


