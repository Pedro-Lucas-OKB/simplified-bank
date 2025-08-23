using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SimplifiedBank.Application.Services.Notification;

namespace SimplifiedBank.Infrastructure.Notifications;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;
    
    public EmailService(IOptions<EmailConfiguration> emailConfiguration)
    {
        _emailConfiguration = emailConfiguration.Value;
    }
    
    public async Task<bool> SendEmailAsync(
        string toName, 
        string toEmail, 
        string subject, 
        string body)
    {
        using var smtpClient = new SmtpClient();
        var emailMessage = new MailMessage
        {
            From = new MailAddress(_emailConfiguration.FromEmail, _emailConfiguration.FromName),
            To = { new MailAddress(toEmail, toName) },
            Subject = subject,
            Body = body
        };

        smtpClient.Host = _emailConfiguration.SmtpHost;
        smtpClient.Port = _emailConfiguration.SmtpPort;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(
            _emailConfiguration.SmtpUserName,
            _emailConfiguration.SmtpPassword);
        smtpClient.EnableSsl = false;
        
        await smtpClient.SendMailAsync(emailMessage);
        
        return true;       
    }
}
