using Microsoft.EntityFrameworkCore;
using Pharm_api.Data;
using Pharm_api.DTOs;
using Pharm_api.Models;

namespace Pharm_api.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly PharmDbContext _context;

        public EmpleadoRepository(PharmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmpleadoDto>> GetEmpleadosByUsuarioAsync(int usuarioId)
        {
            // Obtener sucursales del usuario
            var sucursalIds = await _context.Grupsucursales
                .Where(gs => gs.CodUsuario == usuarioId && gs.Activo)
                .Select(gs => gs.CodSucursal)
                .ToListAsync();

            return await _context.Empleados
                .Include(e => e.CodTipoEmpleadoNavigation)
                .Include(e => e.CodTipoDocumentoNavigation)
                .Include(e => e.CodSucursalNavigation)
                .Where(e => sucursalIds.Contains(e.CodSucursal))
                .Select(e => new EmpleadoDto
                {
                    CodEmpleado = e.CodEmpleado,
                    NomEmpleado = e.NomEmpleado,
                    ApeEmpleado = e.ApeEmpleado,
                    NroTel = e.NroTel,
                    Calle = e.Calle,
                    Altura = e.Altura,
                    Email = e.Email,
                    FechaIngreso = e.FechaIngreso,
                    CodTipoEmpleado = e.CodTipoEmpleado,
                    TipoEmpleado = e.CodTipoEmpleadoNavigation.Tipo,
                    CodTipoDocumento = e.CodTipoDocumento,
                    TipoDocumento = e.CodTipoDocumentoNavigation.Tipo,
                    CodSucursal = e.CodSucursal,
                    NomSucursal = e.CodSucursalNavigation.NomSucursal
                })
                .ToListAsync();
        }

        public async Task<EmpleadoDto?> GetEmpleadoByIdAsync(int codEmpleado, int usuarioId)
        {
            // Verificar que el usuario tenga acceso a la sucursal del empleado
            var sucursalIds = await _context.Grupsucursales
                .Where(gs => gs.CodUsuario == usuarioId && gs.Activo)
                .Select(gs => gs.CodSucursal)
                .ToListAsync();

            return await _context.Empleados
                .Include(e => e.CodTipoEmpleadoNavigation)
                .Include(e => e.CodTipoDocumentoNavigation)
                .Include(e => e.CodSucursalNavigation)
                .Where(e => e.CodEmpleado == codEmpleado && sucursalIds.Contains(e.CodSucursal))
                .Select(e => new EmpleadoDto
                {
                    CodEmpleado = e.CodEmpleado,
                    NomEmpleado = e.NomEmpleado,
                    ApeEmpleado = e.ApeEmpleado,
                    NroTel = e.NroTel,
                    Calle = e.Calle,
                    Altura = e.Altura,
                    Email = e.Email,
                    FechaIngreso = e.FechaIngreso,
                    CodTipoEmpleado = e.CodTipoEmpleado,
                    TipoEmpleado = e.CodTipoEmpleadoNavigation.Tipo,
                    CodTipoDocumento = e.CodTipoDocumento,
                    TipoDocumento = e.CodTipoDocumentoNavigation.Tipo,
                    CodSucursal = e.CodSucursal,
                    NomSucursal = e.CodSucursalNavigation.NomSucursal
                })
                .FirstOrDefaultAsync();
        }

        public async Task<EmpleadoDto?> CreateEmpleadoAsync(CreateEmpleadoDto createDto, int usuarioId)
        {
            // Verificar que el usuario tenga acceso a la sucursal
            var tieneAcceso = await _context.Grupsucursales
                .AnyAsync(gs => gs.CodUsuario == usuarioId && gs.CodSucursal == createDto.CodSucursal && gs.Activo);
            
            if (!tieneAcceso) return null;

            var empleado = new Empleado
            {
                NomEmpleado = createDto.NomEmpleado,
                ApeEmpleado = createDto.ApeEmpleado,
                NroTel = createDto.NroTel,
                Calle = createDto.Calle,
                Altura = createDto.Altura,
                Email = createDto.Email,
                FechaIngreso = createDto.FechaIngreso,
                CodTipoEmpleado = createDto.CodTipoEmpleado,
                CodTipoDocumento = createDto.CodTipoDocumento,
                CodSucursal = createDto.CodSucursal
            };

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return await GetEmpleadoByIdAsync(empleado.CodEmpleado, usuarioId);
        }

        public async Task<EmpleadoDto?> UpdateEmpleadoAsync(int codEmpleado, UpdateEmpleadoDto updateDto, int usuarioId)
        {
            // Verificar que el empleado existe y el usuario tiene acceso
            var empleadoExistente = await GetEmpleadoByIdAsync(codEmpleado, usuarioId);
            if (empleadoExistente == null) return null;

            // Verificar acceso a la nueva sucursal si cambia
            var tieneAcceso = await _context.Grupsucursales
                .AnyAsync(gs => gs.CodUsuario == usuarioId && gs.CodSucursal == updateDto.CodSucursal && gs.Activo);
            
            if (!tieneAcceso) return null;

            var empleado = await _context.Empleados.FindAsync(codEmpleado);
            if (empleado == null) return null;

            empleado.NomEmpleado = updateDto.NomEmpleado;
            empleado.ApeEmpleado = updateDto.ApeEmpleado;
            empleado.NroTel = updateDto.NroTel;
            empleado.Calle = updateDto.Calle;
            empleado.Altura = updateDto.Altura;
            empleado.Email = updateDto.Email;
            empleado.CodTipoEmpleado = updateDto.CodTipoEmpleado;
            empleado.CodTipoDocumento = updateDto.CodTipoDocumento;
            empleado.CodSucursal = updateDto.CodSucursal;

            await _context.SaveChangesAsync();
            return await GetEmpleadoByIdAsync(codEmpleado, usuarioId);
        }

        public async Task<bool> DeleteEmpleadoAsync(int codEmpleado, int usuarioId)
        {
            // Verificar que el empleado existe y el usuario tiene acceso
            var empleadoDto = await GetEmpleadoByIdAsync(codEmpleado, usuarioId);
            if (empleadoDto == null) return false;

            var empleado = await _context.Empleados.FindAsync(codEmpleado);
            if (empleado == null) return false;

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TiposEmpleado>> GetTiposEmpleadoAsync()
        {
            return await _context.TiposEmpleados.ToListAsync();
        }

        public async Task<IEnumerable<TiposDocumento>> GetTiposDocumentoAsync()
        {
            return await _context.TiposDocumentos.ToListAsync();
        }

        public async Task<IEnumerable<Sucursale>> GetSucursalesByUsuarioAsync(int usuarioId)
        {
            var sucursalIds = await _context.Grupsucursales
                .Where(gs => gs.CodUsuario == usuarioId && gs.Activo)
                .Select(gs => gs.CodSucursal)
                .ToListAsync();

            return await _context.Sucursales
                .Where(s => sucursalIds.Contains(s.CodSucursal))
                .ToListAsync();
        }

        // Métodos de validación
        public async Task<bool> EmpleadoExistsAsync(int codEmpleado)
        {
            return await _context.Empleados.AnyAsync(e => e.CodEmpleado == codEmpleado);
        }

        public async Task<bool> EmpleadoExistsForUserAsync(int codEmpleado, int usuarioId)
        {
            var sucursalIds = await _context.Grupsucursales
                .Where(gs => gs.CodUsuario == usuarioId && gs.Activo)
                .Select(gs => gs.CodSucursal)
                .ToListAsync();

            return await _context.Empleados
                .AnyAsync(e => e.CodEmpleado == codEmpleado && sucursalIds.Contains(e.CodSucursal));
        }

        public async Task<bool> SucursalExistsForUserAsync(int codSucursal, int usuarioId)
        {
            return await _context.Grupsucursales
                .AnyAsync(gs => gs.CodSucursal == codSucursal && gs.CodUsuario == usuarioId && gs.Activo);
        }

        public async Task<bool> TipoEmpleadoExistsAsync(int codTipoEmpleado)
        {
            return await _context.TiposEmpleados.AnyAsync(te => te.CodTipoEmpleado == codTipoEmpleado);
        }

        public async Task<bool> TipoDocumentoExistsAsync(int codTipoDocumento)
        {
            return await _context.TiposDocumentos.AnyAsync(td => td.CodTipoDocumento == codTipoDocumento);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeEmpleadoId = null)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var query = _context.Empleados.Where(e => e.Email == email);
            
            if (excludeEmpleadoId.HasValue)
                query = query.Where(e => e.CodEmpleado != excludeEmpleadoId.Value);

            return await query.AnyAsync();
        }
    }
}