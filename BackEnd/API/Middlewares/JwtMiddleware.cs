using SoftOS.Shared.Utils;

namespace SoftOS.API.Middlewares
{
    public class JwtMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Response.Headers.Authorization.Any(string.IsNullOrEmpty))
                context.Response.Headers.Remove("Authorization");

            if (!JwtUtil.TokenValido(context)) { }

            await _next(context);
        }
    }
}
