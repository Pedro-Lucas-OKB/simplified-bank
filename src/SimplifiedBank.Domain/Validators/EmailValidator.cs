using System.Net.Mail;

namespace SimplifiedBank.Domain.Validators;

public abstract class EmailValidator
{
    public static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith('.')) {
            return false;
        }

        if (!trimmedEmail.Contains('@') || !trimmedEmail.Contains('.'))
        {
            return false;       
        }
        
        try {
            var addr = new MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch {
            return false;
        }
    }
}