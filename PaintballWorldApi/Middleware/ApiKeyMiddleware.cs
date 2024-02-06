using PaintballWorld.Core.Interfaces;

namespace PaintballWorld.API.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private readonly IApiKeyService _apiKeyService;
        private const string ApiKeyHeaderName = "PW_API_KEY";

        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger, IApiKeyService apiKeyService)
        {
            _next = next;
            _logger = logger;
            _apiKeyService = apiKeyService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                _logger.LogWarning("Zapytanie bez klucza API.");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            if (!_apiKeyService.IsApiKeyValid(extractedApiKey.ToString()))
            {
                _logger.LogWarning("Nieautoryzowane zapytanie z nieprawidłowym kluczem API.");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            _logger.LogInformation("Prawidłowe zapytanie z kluczem API.");
            await _next(context);
        }
    }

}
