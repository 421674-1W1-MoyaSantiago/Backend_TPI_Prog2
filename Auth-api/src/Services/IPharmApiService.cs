namespace Auth_api.Services
{
    public interface IPharmApiService
    {
        Task<bool> CreateUserInPharmApiAsync(int userId, string username, string email);
    }
}