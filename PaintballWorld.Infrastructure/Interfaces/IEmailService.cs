namespace PaintballWorld.Infrastructure.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, params int[] attachments);
    Task FetchEmailsAsync(string query);
    Task SendResetPasswordEmailAsync(string email, string callbackUrl);
    Task SendConfirmationEmailAsync(string email, string? callbackUrl);
}