using Microsoft.Identity.Client;
using PaintballWorld.API.Areas.Schedule.Controllers;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Core.Services;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Services;

namespace PaintballWorld.API
{
    public static class DependencyInjector
    {
        public static void Inject(this IServiceCollection services)
        {
            // Infrastructure
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthTokenService, AuthTokenService>();


            // Domain
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IApiKeyService, ApiKeyService>();
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IOwnerRegistrationService, OwnerRegistrationService>();
            services.AddScoped<IAutocompleteService, AutocompleteService>();
            services.AddScoped<IFieldManagementService, FieldManagementService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IReservationService, ReservationService>();


        }
    }
}
