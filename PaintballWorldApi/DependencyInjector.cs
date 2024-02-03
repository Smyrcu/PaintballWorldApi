using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Services;

namespace PaintballWorld.API
{
    public static class DependencyInjector
    {
        public static void Inject(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthTokenService, AuthTokenService>();
            
        }
    }
}
