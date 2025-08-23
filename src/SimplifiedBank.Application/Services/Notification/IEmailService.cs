namespace SimplifiedBank.Application.Services.Notification;

public interface IEmailService
{
    Task<bool> SendEmailAsync(
        string toName,
        string toEmail,
        string subject,
        string body);
}