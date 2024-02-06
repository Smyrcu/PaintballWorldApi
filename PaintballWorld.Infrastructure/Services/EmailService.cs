using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net.Mail;
using MimeKit;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;
using Azure;
using Microsoft.EntityFrameworkCore;
using System.Text;
using MessagePart = Google.Apis.Gmail.v1.Data.MessagePart;
using System.Net.Http;
using System.Net;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using Attachment = System.Net.Mail.Attachment;

namespace PaintballWorld.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFileService _fileService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmailService> _logger;
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SmtpUsername = "paintballworldpw@gmail.com";
        private const string SmtpPassword = "ugyd uwlg vlxe xkos";

        private readonly SmtpClient _smtpClient;
        //SVKzaMARRpbZZ6K5I6bI


        public EmailService(IFileService fileService, ApplicationDbContext context, ILogger<EmailService> logger)
        {
            _fileService = fileService;
            _context = context;
            _logger = logger;
            _smtpClient = new SmtpClient(SmtpServer, SmtpPort)
            {
                Credentials = new NetworkCredential(SmtpUsername, SmtpPassword),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string to, string subject, string body, params int[] attachmentIds)
        {
            using var message = new MailMessage();
            message.From = new MailAddress(SmtpUsername);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;

            foreach (var attachmentId in attachmentIds)
            {
                var attachment = GetAttachment(attachmentId);
                message.Attachments.Add(attachment);
            }

            try
            {
                await _smtpClient.SendMailAsync(message);
                var outboxMessage = new EmailOutbox
                {
                    Recipient = to,
                    Subject = subject,
                    Body = body,
                    SentTime = DateTime.UtcNow,
                    IsSent = true,
                    SendTries = 1
                };
                await _context.EmailOutboxes.AddAsync(outboxMessage);
                _logger.LogInformation("Wiadomość została wysłana pomyślnie.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas wysyłania wiadomości.");
            }
        }

        private Attachment GetAttachment(int attachmentId)
        {
            var filePath = _fileService.GetAttachmentPathById(attachmentId);

            return new Attachment(filePath);
        }

        //public async Task FetchEmailsAsync(string query)
        //{
        //    var request = _service.Users.Messages.List("me");
        //    request.Q = query;

        //    var response = await request.ExecuteAsync();
        //    var messages = response.Messages;

        //    foreach (var messageSummary in messages)
        //    {
        //        var emailMessageRequest = _service.Users.Messages.Get("me", messageSummary.Id);
        //        var emailMessage = await emailMessageRequest.ExecuteAsync();

        //        DateTime.TryParse(emailMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value, out var date);

        //        var emailInbox = new EmailInbox
        //        {
        //            MessageId = emailMessage.Id,
        //            Sender = emailMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value,
        //            Subject = emailMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value,
        //            Body = GetFullMessageBody(emailMessage.Payload),
        //            ReceivedTime = date,
        //            IsRead = false
        //        };

        //        var existingEmail = await _context.EmailInboxes.FirstOrDefaultAsync(e => e.MessageId == emailMessage.Id);
        //        if (existingEmail == null)
        //        {
        //            await _context.EmailInboxes.AddAsync(emailInbox);
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //}

        public async Task SendResetPasswordEmailAsync(string email, string callbackUrl)
        {
            //TODO: Wyciągnąć to z bazy :)
            var body = "";
            var subject = $"{callbackUrl}";
            await this.SendEmailAsync(email, subject, body);
        }

        public async Task SendConfirmationEmailAsync(string email, string? callbackUrl)
        {
            //TODO: Wyciągnąć to z bazy :)
            var body = "";
            var subject = $"{callbackUrl}";
            await this.SendEmailAsync(email, subject, body);
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return System.Convert.ToBase64String(inputBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }

        private static string Base64UrlDecode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            switch (input.Length % 4)
            {
                case 2: input += "=="; break;
                case 3: input += "="; break;
            }
            var inputBytes = Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/'));
            return System.Text.Encoding.UTF8.GetString(inputBytes);
        }

        private static string GetFullMessageBody(MessagePart payload)
        {
            if (payload.Parts == null || !payload.Parts.Any())
            {
                return payload.Body.Data; 
            }

            var fullBody = new StringBuilder();
            foreach (var part in payload.Parts)
            {
                if (part.MimeType == "text/plain")
                {
                    fullBody.Append(Base64UrlDecode(part.Body.Data));
                }
            }
            return fullBody.ToString();
        }
    }
}
