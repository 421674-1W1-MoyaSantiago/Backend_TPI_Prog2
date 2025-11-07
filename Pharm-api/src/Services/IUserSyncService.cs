using Pharm_api.DTOs;

namespace Pharm_api.Services
{
    public interface IUserSyncService
    {
        Task<UserSyncResponseDto> CreateUserFromAuthAsync(CreateUserFromAuthDto dto);
        Task<bool> UserExistsAsync(int userId);
    }

    public class UserSyncResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<int> SucursalesAsignadas { get; set; } = new();
        public int TotalSucursales => SucursalesAsignadas.Count;
    }
}