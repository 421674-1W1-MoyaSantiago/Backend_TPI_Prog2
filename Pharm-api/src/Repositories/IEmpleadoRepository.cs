using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories;

public interface IEmpleadoRepository
{
    // Métodos existentes con filtro por usuario
    Task<IEnumerable<EmpleadoDto>> GetEmpleadosByUsuarioAsync(int usuarioId);
    Task<EmpleadoDto?> GetEmpleadoByIdAsync(int codEmpleado, int usuarioId);
    Task<EmpleadoDto?> CreateEmpleadoAsync(CreateEmpleadoDto createDto, int usuarioId);
    Task<EmpleadoDto?> UpdateEmpleadoAsync(int codEmpleado, UpdateEmpleadoDto updateDto, int usuarioId);
    Task<bool> DeleteEmpleadoAsync(int codEmpleado, int usuarioId);
    
    // Métodos de consulta adicionales
    Task<IEnumerable<TiposEmpleado>> GetTiposEmpleadoAsync();
    Task<IEnumerable<TiposDocumento>> GetTiposDocumentoAsync();
    Task<IEnumerable<Sucursale>> GetSucursalesByUsuarioAsync(int usuarioId);
    
    // Métodos de validación
    Task<bool> EmpleadoExistsAsync(int codEmpleado);
    Task<bool> EmpleadoExistsForUserAsync(int codEmpleado, int usuarioId);
    Task<bool> SucursalExistsForUserAsync(int codSucursal, int usuarioId);
    Task<bool> TipoEmpleadoExistsAsync(int codTipoEmpleado);
    Task<bool> TipoDocumentoExistsAsync(int codTipoDocumento);
    Task<bool> EmailExistsAsync(string email, int? excludeEmpleadoId = null);
}
