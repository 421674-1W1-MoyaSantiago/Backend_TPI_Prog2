using Pharm_api.DTOs;
using Pharm_api.Repositories;

namespace Pharm_api.Services
{
    public class UserSyncService : IUserSyncService
    {
        private readonly IUserSyncRepository _userSyncRepository;
        private readonly ILogger<UserSyncService> _logger;

        public UserSyncService(IUserSyncRepository userSyncRepository, ILogger<UserSyncService> logger)
        {
            _userSyncRepository = userSyncRepository;
            _logger = logger;
        }

        public async Task<UserSyncResponseDto> CreateUserFromAuthAsync(CreateUserFromAuthDto dto)
        {
            try
            {
                _logger.LogInformation($"Iniciando sincronización de usuario desde Auth-api: {dto.Username} (ID: {dto.UserId})");

                // Verificar si el usuario ya existe
                var existingUser = await _userSyncRepository.GetUsuarioByIdAsync(dto.UserId);
                if (existingUser != null)
                {
                    _logger.LogInformation($"Usuario {dto.Username} ya existe en PharmDB");
                    return new UserSyncResponseDto
                    {
                        Success = true,
                        Message = "Usuario ya existe en PharmDB",
                        UserId = dto.UserId,
                        Username = dto.Username,
                        SucursalesAsignadas = new List<int> { 1, 2, 3 } // Asumimos que ya tiene las sucursales
                    };
                }

                // Crear el usuario
                var usuario = await _userSyncRepository.CreateUsuarioAsync(dto.UserId, dto.Username, dto.Email);

                // Asignar las 3 sucursales por defecto
                var sucursalesAsignadas = await _userSyncRepository.AsignarSucursalesDefaultAsync(dto.UserId);

                if (sucursalesAsignadas)
                {
                    _logger.LogInformation($"Usuario {dto.Username} creado exitosamente con 3 sucursales asignadas");
                    return new UserSyncResponseDto
                    {
                        Success = true,
                        Message = "Usuario creado exitosamente en PharmDB con sucursales asignadas",
                        UserId = dto.UserId,
                        Username = dto.Username,
                        SucursalesAsignadas = new List<int> { 1, 2, 3 }
                    };
                }
                else
                {
                    _logger.LogWarning($"Usuario {dto.Username} creado pero falló la asignación de sucursales");
                    return new UserSyncResponseDto
                    {
                        Success = false,
                        Message = "Usuario creado pero falló la asignación de sucursales",
                        UserId = dto.UserId,
                        Username = dto.Username,
                        SucursalesAsignadas = new List<int>()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creando usuario {dto.Username} desde Auth-api");
                return new UserSyncResponseDto
                {
                    Success = false,
                    Message = $"Error interno: {ex.Message}",
                    UserId = dto.UserId,
                    Username = dto.Username,
                    SucursalesAsignadas = new List<int>()
                };
            }
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            var user = await _userSyncRepository.GetUsuarioByIdAsync(userId);
            return user != null;
        }
    }
}