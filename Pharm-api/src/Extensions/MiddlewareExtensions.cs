using Pharm_api.Middleware;

namespace Pharm_api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAutoCreateUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AutoCreateUserMiddleware>();
        }
    }
}