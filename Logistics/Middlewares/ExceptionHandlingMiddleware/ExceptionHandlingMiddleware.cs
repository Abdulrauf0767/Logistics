using System.Net;
using System.Text.Json;

namespace Logistics.Middlewares.ExceptionHandlingMiddleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next , ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex) {
                _logger.LogError(ex, "An unhandled exception occurred during the request.");
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Default error state (Internal Server Error)
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var title = "Internal Server Error";
            var detail = exception.Message;

            if (exception is BadHttpRequestException badRequestEx)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                title = "Validation Error";
                detail = badRequestEx.Message;
            }

            var responseJson = JsonSerializer.Serialize(new
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            });

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(responseJson);
        }
    }
}
