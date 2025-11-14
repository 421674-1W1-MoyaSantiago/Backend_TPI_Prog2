using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Repositories;

namespace Pharm_api.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepository _repository;

        public EmpleadoService(IEmpleadoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EmpleadoDto>> GetEmpleadosByUsuarioAsync(int usuarioId)
        {
            return await _repository.GetEmpleadosByUsuarioAsync(usuarioId);
        }

        public async Task<EmpleadoDto?> GetEmpleadoByIdAsync(int codEmpleado, int usuarioId)
        {
            return await _repository.GetEmpleadoByIdAsync(codEmpleado, usuarioId);
        }

        public async Task<EmpleadoDto?> CreateEmpleadoAsync(CreateEmpleadoDto createDto, int usuarioId)
        {
            // Validaciones de negocio
            await ValidateEmployeeDataAsync(createDto, usuarioId);

            // Verificar email único si se proporciona
            if (!string.IsNullOrEmpty(createDto.Email))
            {
                var emailExists = await _repository.EmailExistsAsync(createDto.Email);
                if (emailExists)
                {
                    throw new InvalidOperationException("Ya existe un empleado con este email");
                }
            }

            return await _repository.CreateEmpleadoAsync(createDto, usuarioId);
        }

        public async Task<EmpleadoDto?> UpdateEmpleadoAsync(int codEmpleado, UpdateEmpleadoDto updateDto, int usuarioId)
        {
            // Verificar que el empleado existe y pertenece al usuario
            var empleadoExists = await _repository.EmpleadoExistsForUserAsync(codEmpleado, usuarioId);
            if (!empleadoExists)
            {
                throw new InvalidOperationException("Empleado no encontrado o no tienes acceso a él");
            }

            // Validaciones de negocio (convertir UpdateDto a CreateDto para reutilizar validación)
            var createDto = new CreateEmpleadoDto
            {
                NroTel = updateDto.NroTel,
                Calle = updateDto.Calle,
                Altura = updateDto.Altura,
                Email = updateDto.Email,
                CodTipoEmpleado = updateDto.CodTipoEmpleado,
                CodSucursal = updateDto.CodSucursal
            };
            
            await ValidateEmployeeDataAsync(createDto, usuarioId, codEmpleado);

            // Verificar email único si se proporciona
            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                var emailExists = await _repository.EmailExistsAsync(updateDto.Email, codEmpleado);
                if (emailExists)
                {
                    throw new InvalidOperationException("Ya existe otro empleado con este email");
                }
            }

            return await _repository.UpdateEmpleadoAsync(codEmpleado, updateDto, usuarioId);
        }

        public async Task<bool> DeleteEmpleadoAsync(int codEmpleado, int usuarioId)
        {
            // Verificar que el empleado existe y pertenece al usuario
            var empleadoExists = await _repository.EmpleadoExistsForUserAsync(codEmpleado, usuarioId);
            if (!empleadoExists)
            {
                throw new InvalidOperationException("Empleado no encontrado o no tienes acceso a él");
            }

            return await _repository.DeleteEmpleadoAsync(codEmpleado, usuarioId);
        }

        public async Task<IEnumerable<TiposEmpleado>> GetTiposEmpleadoAsync()
        {
            return await _repository.GetTiposEmpleadoAsync();
        }

        public async Task<IEnumerable<TiposDocumento>> GetTiposDocumentoAsync()
        {
            return await _repository.GetTiposDocumentoAsync();
        }

        public async Task<IEnumerable<Sucursale>> GetSucursalesByUsuarioAsync(int usuarioId)
        {
            return await _repository.GetSucursalesByUsuarioAsync(usuarioId);
        }

        // Método privado para validaciones comunes
        private async Task ValidateEmployeeDataAsync(CreateEmpleadoDto createDto, int usuarioId, int? excludeEmpleadoId = null)
        {
            // Verificar que la sucursal existe y pertenece al usuario
            var sucursalExists = await _repository.SucursalExistsForUserAsync(createDto.CodSucursal, usuarioId);
            if (!sucursalExists)
            {
                throw new InvalidOperationException("La sucursal especificada no existe o no tienes acceso a ella");
            }

            // Verificar que el tipo de empleado existe
            var tipoEmpleadoExists = await _repository.TipoEmpleadoExistsAsync(createDto.CodTipoEmpleado);
            if (!tipoEmpleadoExists)
            {
                throw new InvalidOperationException("El tipo de empleado especificado no existe");
            }

            // Verificar que el tipo de documento existe
            var tipoDocumentoExists = await _repository.TipoDocumentoExistsAsync(createDto.CodTipoDocumento);
            if (!tipoDocumentoExists)
            {
                throw new InvalidOperationException("El tipo de documento especificado no existe");
            }

            // Validar fecha de ingreso (no puede ser futura)
            if (createDto.FechaIngreso > DateTime.Now)
            {
                throw new InvalidOperationException("La fecha de ingreso no puede ser futura");
            }
        }
    }
}