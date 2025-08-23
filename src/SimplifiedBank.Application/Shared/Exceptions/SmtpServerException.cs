namespace SimplifiedBank.Application.Shared.Exceptions;

public class SmtpServerException : Exception
{
    public SmtpServerException(Exception innerException, string email, string action) 
        : base($"Falha ao enviar email para {email} ao realizar {action}.", innerException) { }   
}