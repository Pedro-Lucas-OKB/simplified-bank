using SimplifiedBank.Domain.Exceptions;

namespace SimplifiedBank.Domain.Entities;

public class Transaction : BaseEntity
{
    private Transaction() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    public Guid SenderId { get; init; }
    public User Sender { get; private set; } = null!;
    
    public Guid ReceiverId { get; init; }
    public User Receiver { get; private set; } = null!;
    public decimal Value { get; init; }

    /// <summary>
    /// Método Factory
    /// </summary>
    /// <param name="senderId"></param>
    /// <param name="receiverId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    /// <exception cref="InvalidTransactionValueException"></exception>
    public static Transaction Create(
        Guid senderId,
        Guid receiverId,
        decimal value)
    {
        if (senderId == Guid.Empty)
            throw new DomainException("ID de pagador é inválido.");
        
        if (receiverId == Guid.Empty)
            throw new DomainException("ID de recebedor é inválido.");

        if (senderId == receiverId)
            throw new SenderAndReceiverCannotBeEqualException("Um usuário não pode realizar uma transação para si mesmo.");
        
        switch (value)
        {
            case < DomainConfiguration.MinTransactionValue:
                throw new InvalidTransactionValueException($"O valor deve ser maior ou igual a {DomainConfiguration.MinTransactionValue}.");
            case > DomainConfiguration.MaxTransactionValue:
                throw new InvalidTransactionValueException($"O valor deve ser menor ou igual a {DomainConfiguration.MaxTransactionValue}.");
        }
        
        return new Transaction
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Value = value,
        };
    }
}
