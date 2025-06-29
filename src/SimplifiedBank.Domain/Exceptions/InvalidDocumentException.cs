namespace SimplifiedBank.Domain.Exceptions;

public class InvalidDocumentException : DomainException
{
    public InvalidDocumentException()
    {
        
    }
    
    public InvalidDocumentException(string message) 
        : base(message) { }
}