using Pharm_api.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pharm_api.Services
{
    public interface IUsuarioService
    {
    Task<string> GenerateTokenAsync(LoginDto loginDto);
    Task<bool> ValidateUserAsync(LoginDto loginDto);
    Task<UsuarioDto> CreateUsuarioAsync(CreateUsuarioDto createDto);
    Task AsignarSucursalesAsync(string username, List<int> sucursalesIds);
    Task<List<SucursalDto>> GetSucursalesUsuarioAsync(string username);
    Task<UsuarioDto?> GetByUsernameAsync(string username);
    Task<List<UsuarioDto>> GetAllUsuariosAsync();
    int? GetUserIdFromToken(HttpContext httpContext);
    string? GetUsernameFromToken(HttpContext httpContext);
    string? GetEmailFromToken(HttpContext httpContext);
    }
}