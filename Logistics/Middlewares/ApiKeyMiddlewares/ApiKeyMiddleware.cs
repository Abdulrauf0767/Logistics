namespace Logistics.Middlewares.ApiKeyMiddlware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            if (!context.Request.Headers.TryGetValue("ApiKey", out var extractedApiKey))
            {
                _logger.LogWarning("Api is missing");
                throw new BadHttpRequestException("Api is missing");
            }
            var apiKey = configuration.GetValue<string>("ApiKey");
            if (string.IsNullOrEmpty(apiKey) || !apiKey.Equals(extractedApiKey))
            {
                _logger.LogWarning("Invalid API Key attempted.");
                throw new BadHttpRequestException("Unauthorized client. Invalid API Key.");
            }
            await _next(context);
        }

    }
}