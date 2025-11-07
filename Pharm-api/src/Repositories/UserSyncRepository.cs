using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;
using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public class UserSyncRepository : IUserSyncRepository
    {
        private readonly PharmDbContext _context;
        private readonly ILogger<UserSyncRepository> _logger;

        public UserSyncRepository(PharmDbContext context, ILogger<UserSyncRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int userId)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.CodUsuario == userId);
        }

        public async Task<Usuario> CreateUsuarioAsync(int userId, string username, string email)
        {
            var usuario = new Usuario
            {
                CodUsuario = userId,
                Username = username,
                Email = email
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Usuario {username} (ID: {userId}) creado en PharmDB");
            return usuario;
        }

        public async Task<bool> AsignarSucursalesDefaultAsync(int userId)
        {
            try
            {
                // Verificar que el usuario existe
                var usuario = await GetUsuarioByIdAsync(userId);
                if (usuario == null)
                {
                    _logger.LogWarning($"Usuario con ID {userId} no encontrado para asignar sucursales");
                    return false;
                }

                // Obtener sucursales default (1, 2, 3)
                var sucursalesDefault = await _context.Sucursales
                    .Where(s => s.CodSucursal <= 3)
                    .ToListAsync();

                foreach (var sucursal in sucursalesDefault)
                {
                    // Verificar que no existe ya la asignación
                    var exists = await _context.Grupsucursales
                        .AnyAsync(g => g.CodUsuario == userId && g.CodSucursal == sucursal.CodSucursal);

                    if (!exists)
                    {
                        var asignacion = new Grupsucursale
                        {
                            CodUsuario = userId,
                            CodSucursal = sucursal.CodSucursal,
                            Activo = true
                        };
                        
                        _context.Grupsucursales.Add(asignacion);
                        _logger.LogInformation($"Asignada sucursal {sucursal.CodSucursal} al usuario {userId}");
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error asignando sucursales default al usuario {userId}");
                return false;
            }
        }

        public async Task<List<Sucursale>> GetSucursalesDefaultAsync()
        {
            return await _context.Sucursales
                .Where(s => s.CodSucursal <= 3)
                .ToListAsync();
        }
    }
}