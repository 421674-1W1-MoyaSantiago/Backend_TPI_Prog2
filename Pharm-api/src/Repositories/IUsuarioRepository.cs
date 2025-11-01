using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario> CreateUsuarioAsync(CreateUsuarioDto createDto);
    Task<Usuario?> GetUsuarioByUsernameAsync(string username);
    Task<Usuario?> GetUsuarioByIdAsync(int id);
    Task AsignarSucursalesAsync(string username, List<int> sucursales);
    Task<IEnumerable<UsuarioSucursalDto>> GetSucursalesUsuarioAsync(string username);
    Task<bool> UsuarioExistsAsync(string username);
    Task<List<Usuario>> GetAllUsuariosAsync();
}