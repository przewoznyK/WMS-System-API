using System.Text.Json;
using WMS.Domain.Exceptions;

namespace WMS.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if(ex is WmsException wmsException)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = wmsException.StatusCode;
                    var error = new { message = "Aplication error", details = wmsException.Message };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                }
                else
                {
                    _logger.LogError(ex, "Unexpected server error");

                    context.Response.StatusCode = 500;
                    var error = new { message = "Internal server error", details = ex.Message };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                }
            }
        }
    }
}