
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using PaintballWorld.Infrastructure;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Tutaj dodajemy nasze serwisy do kontenera DI
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthTokenService, AuthTokenService>();

        services.AddDbContext<ApplicationDbContext>();

        services.AddLogging(logger =>
        {
            logger.ClearProviders();
            logger.SetMinimumLevel(LogLevel.Trace);
            logger.AddConsole();
        });
    })
    .Build();

// Zdefiniuj, jak użyć serwisu emailowego
async Task RunEmailService(IHost host)
{
    var emailService = host.Services.GetRequiredService<IEmailService>();

    // Tutaj używamy serwisu do wysłania emaila
    await emailService.SendEmailAsync("blszadkowski@gmail.com", "Test Email", "This is a test email body.");
}

// Uruchamiamy naszą aplikację
await RunEmailService(host);

Console.Read();
