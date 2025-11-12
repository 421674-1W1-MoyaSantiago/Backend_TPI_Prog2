namespace Pharm_api.DTOs
{
    public class TopMedicamentoYVendedorPorEstacionDto
    {
        public string Estacion { get; set; }
        public string MedicamentoMasVendido { get; set; }
        public string Vendedor { get; set; }
        public int TotalVendido { get; set; }
    }

}
