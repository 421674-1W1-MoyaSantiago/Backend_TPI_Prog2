using Auth_api.Models;
using Auth_api.Repositories;
using Auth_api.UserDTOs;

namespace Auth_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IJwtService jwtService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task CreateAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task SoftDeleteAsync(int id)
        {
            await _userRepository.SoftDeleteAsync(id);
        }

        public async Task<LoginResponseDto> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            
            if (user == null || !user.IsActive)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // Verificar la contraseña usando BCrypt
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // Generar token
            var token = _jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                Success = true,
                Message = "Login successful",
                Token = token
            };
        }

        public async Task RestoreAsync(int id)
        {
            await _userRepository.RestoreAsync(id);
        }

        public async Task<User?> GetByIdIncludeDeletedAsync(int id)
        {
            return await _userRepository.GetByIdIncludeDeletedAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
    }
}