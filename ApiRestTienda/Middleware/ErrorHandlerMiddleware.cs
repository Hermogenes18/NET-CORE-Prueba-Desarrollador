using System.Net;
using System.Text.Json;

namespace ApiRestTienda.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                _logger.LogError(error, error.Message);

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }

    
    public static class ErrorHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
