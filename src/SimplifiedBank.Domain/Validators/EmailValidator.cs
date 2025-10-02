using System.Net.Mail;

namespace SimplifiedBank.Domain.Validators;

public abstract class EmailValidator
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;       
        }
        
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith('.')) {
            return false;
        }

        if (trimmedEmail.Contains(' ') || trimmedEmail.Contains('\t') || trimmedEmail.Contains('\n'))
        {
            return false;      
        }

        if (!trimmedEmail.Contains('@') || !trimmedEmail.Contains('.'))
        {
            return false;       
        }

        if (trimmedEmail.Contains(".@") || trimmedEmail.Contains("@."))
        {
            return false;      
        }
        
        var splitedEmail = trimmedEmail.Split('.');
        if (splitedEmail.Any(x => x == string.Empty)) // pontos seguidos
        {
            return false;      
        }
        
        splitedEmail = trimmedEmail.Split('@');
        if (splitedEmail.Any(x => x == string.Empty)) // Arrobas seguidas
        {
            return false;      
        }
        
        try {
            var addr = new MailAddress(trimmedEmail);

            return addr.User.Length <= 64 && addr.Host.Length <= 255;
        }
        catch (FormatException) {
            return false;
        }
    }
}