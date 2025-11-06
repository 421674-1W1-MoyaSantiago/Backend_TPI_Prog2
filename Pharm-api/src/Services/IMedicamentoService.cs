using Pharm_api.DTOs;

namespace Pharm_api.Services
{
    public interface IMedicamentoService
    {
        Task<IEnumerable<MedicamentoDto>> GetAllMedicamentosAsync();
        Task<MedicamentoDto?> GetMedicamentoByIdAsync(int id);
        Task<MedicamentoDto> CreateMedicamentoAsync(CreateMedicamentoDto createDto);
        Task<MedicamentoDto?> UpdateMedicamentoAsync(int id, UpdateMedicamentoDto updateDto);
        Task<bool> DeleteMedicamentoAsync(int id);
        Task<IEnumerable<MedicamentoDto>> GetMedicamentosByLaboratorioAsync(int laboratorioId);
        Task<IEnumerable<MedicamentoDto>> GetMedicamentosByTipoAsync(int tipoMedicamentoId);
        Task<IEnumerable<MedicamentoDto>> SearchMedicamentosByDescripcionAsync(string descripcion);
        Task<IEnumerable<MedicamentoDto>> GetMedicamentosConStockBajoAsync(int cantidadMinima = 10);
        Task<int> GetCountMedicamentosConStockBajoAsync(int cantidadMinima = 10);
        Task<bool> ValidateMedicamentoDataAsync(CreateMedicamentoDto createDto);
        Task<bool> ValidateMedicamentoDataAsync(UpdateMedicamentoDto updateDto);
    }
}