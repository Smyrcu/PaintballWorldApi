using PaintballWorld.Core.Interfaces;

namespace PaintballWorld.API.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private const string ApiKeyHeaderName = "PW_API_KEY";

        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IApiKeyService apiKeyService)
        {
            var excludedPaths = new List<PathString>() 
            {
                new("/swagger"),
                new("/api/Auth/Register/ConfirmAccount"),
                new("/img"),
                new("/regulations"),
            };

            //if (excludedPaths.Any(path => context.Request.Path.StartsWithSegments(new PathString(path))))
            {
                await _next(context);
                return;
            }
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                _logger.LogWarning("Zapytanie bez klucza API.");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            if (!apiKeyService.IsApiKeyValid(extractedApiKey.ToString()))
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
