using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Services
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoDto>> GetEmpleadosByUsuarioAsync(int usuarioId);
        Task<EmpleadoDto?> GetEmpleadoByIdAsync(int codEmpleado, int usuarioId);
        Task<EmpleadoDto?> CreateEmpleadoAsync(CreateEmpleadoDto createDto, int usuarioId);
        Task<EmpleadoDto?> UpdateEmpleadoAsync(int codEmpleado, UpdateEmpleadoDto updateDto, int usuarioId);
        Task<bool> DeleteEmpleadoAsync(int codEmpleado, int usuarioId);
        Task<IEnumerable<TiposEmpleado>> GetTiposEmpleadoAsync();
        Task<IEnumerable<TiposDocumento>> GetTiposDocumentoAsync();
        Task<IEnumerable<Sucursale>> GetSucursalesByUsuarioAsync(int usuarioId);
    }
}