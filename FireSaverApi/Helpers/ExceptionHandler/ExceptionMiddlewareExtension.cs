using Microsoft.AspNetCore.Builder;

namespace FireSaverApi.Helpers.ExceptionHandler
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}