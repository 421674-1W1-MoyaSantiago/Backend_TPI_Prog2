using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public interface IMedicamentoRepository
    {
        Task<IEnumerable<MedicamentoDto>> GetAllAsync();
        Task<MedicamentoDto?> GetByIdAsync(int id);
        Task<MedicamentoDto> CreateAsync(CreateMedicamentoDto createDto);
        Task<MedicamentoDto?> UpdateAsync(int id, UpdateMedicamentoDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<MedicamentoDto>> GetByLaboratorioAsync(int laboratorioId);
        Task<IEnumerable<MedicamentoDto>> GetByTipoMedicamentoAsync(int tipoMedicamentoId);
        Task<IEnumerable<MedicamentoDto>> SearchByDescripcionAsync(string descripcion);
        Task<IEnumerable<MedicamentoDto>> GetMedicamentosConStockBajoAsync(int cantidadMinima = 10);
        Task<int> GetCountMedicamentosConStockBajoAsync(int cantidadMinima = 10);
    }
}