
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using PaintballWorld.Infrastructure;
using System.Net.Mail;
using System.Net;

//var host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices((context, services) =>
//    {
//        // Tutaj dodajemy nasze serwisy do kontenera DI
//        services.AddScoped<IFileService, FileService>();
//        services.AddScoped<IEmailService, EmailService>();
//        services.AddScoped<IAuthTokenService, AuthTokenService>();

//        services.AddDbContext<ApplicationDbContext>();

//        services.AddLogging(logger =>
//        {
//            logger.ClearProviders();
//            logger.SetMinimumLevel(LogLevel.Trace);
//            logger.AddConsole();
//        });
//    })
//    .Build();

//// Zdefiniuj, jak użyć serwisu emailowego
//async Task RunEmailService(IHost host)
//{
//    var emailService = host.Services.GetRequiredService<IEmailService>();

//    // Tutaj używamy serwisu do wysłania emaila
//    await emailService.SendEmailAsync("blszadkowski@gmail.com", "Test Email", "This is a test email body.");
//}

//// Uruchamiamy naszą aplikację
//await RunEmailService(host);
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

string smtpServer = "smtp.gmail.com"; // Adres serwera SMTP Gmail
int smtpPort = 587; // Port SMTP dla Gmail
string smtpUsername = "paintballworldpw@gmail.com"; // Twój adres e-mail Gmail
string smtpPassword = "ugyd uwlg vlxe xkos"; // Hasło do konta Gmail

string recipientEmail = "bl.szadkowski@gmail.com"; // Adres e-mail odbiorcy
string subject = "Temat wiadomości";
string body = "Treść wiadomości";

using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
{
    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
    client.EnableSsl = true; // Wyłącz SSL

    MailMessage message = new MailMessage(smtpUsername, recipientEmail, subject, body);

    try
    {
        await client.SendMailAsync(message);
        Console.WriteLine("Wiadomość została wysłana pomyślnie.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Wystąpił błąd podczas wysyłania wiadomości: " + ex.Message);
    }
}
