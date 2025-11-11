using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;
using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories;

public class UsuarioRepository : IUsuarioRepository
{

    public async Task<List<Usuario>> GetAllUsuariosAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }
    private readonly PharmDbContext _context;

    public UsuarioRepository(PharmDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> CreateUsuarioAsync(CreateUsuarioDto createDto)
    {
        var usuario = new Usuario
        {
            Username = createDto.Username,
            Email = createDto.Email
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<Usuario?> GetUsuarioByUsernameAsync(string username)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == username);
        return usuario;
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.CodUsuario == id);
    }

    public async Task AsignarSucursalesAsync(string username, List<int> sucursales)
    {
        var usuario = await GetUsuarioByUsernameAsync(username);
        if (usuario == null) throw new ArgumentException("Usuario no encontrado");

        // Remover asignaciones anteriores
    var asignacionesAnteriores = await _context.Grupsucursales
            .Where(gs => gs.CodUsuario == usuario.CodUsuario)
            .ToListAsync();
        
    _context.Grupsucursales.RemoveRange(asignacionesAnteriores);

        // Crear nuevas asignaciones
        foreach (var codSucursal in sucursales)
        {
            var grupSucursal = new Grupsucursale
            {
                CodUsuario = usuario.CodUsuario,
                CodSucursal = codSucursal,
                FechaAsignacion = DateTime.UtcNow,
                Activo = true
            };
            _context.Grupsucursales.Add(grupSucursal);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UsuarioSucursalDto>> GetSucursalesUsuarioAsync(string username)
    {
        var usuario = await GetUsuarioByUsernameAsync(username);
        if (usuario == null) return new List<UsuarioSucursalDto>();

        return await _context.Grupsucursales
            .Include(gs => gs.CodSucursalNavigation)
            .Where(gs => gs.CodUsuario == usuario.CodUsuario && gs.Activo)
            .Select(gs => new UsuarioSucursalDto
            {
                CodSucursal = gs.CodSucursal,
                NomSucursal = gs.CodSucursalNavigation.NomSucursal,
                FechaAsignacion = gs.FechaAsignacion,
                Activo = gs.Activo
            })
            .ToListAsync();
    }

    public async Task<bool> UsuarioExistsAsync(string username)
    {
        return await _context.Usuarios.AnyAsync(u => u.Username == username);
    }
}