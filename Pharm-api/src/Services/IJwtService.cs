using Pharm_api.Models;
using Microsoft.AspNetCore.Http;

namespace Pharm_api.Services
{
    public interface IJwtService
    {
        string GenerateToken(Usuario user);
        bool ValidateToken(string token);
        int? GetUserIdFromToken(HttpContext httpContext);
    }
}