using SimplifiedBank.Domain.Enums;
using SimplifiedBank.Domain.Exceptions;
using SimplifiedBank.Domain.Validators;

namespace SimplifiedBank.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;   
    public string PasswordHash { get; private set; } = null!;   
    public string Document { get; private set; } = null!;  
    public decimal Balance { get; private set; }
    public EUserType Type { get; private set; }
    public List<Transaction> TransactionsSent { get; private set; } = null!;
    public List<Transaction> TransactionsReceived { get; private set; } = null!;

    private User() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        
    }

    /// <summary>
    /// Método Factory
    /// </summary>
    /// <param name="fullName"></param>
    /// <param name="email"></param>
    /// <param name="passwordHash"></param>
    /// <param name="document"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    /// <exception cref="InvalidDocumentException"></exception>
    public static User Create(
        string fullName,
        string email,
        string passwordHash,
        string document,
        EUserType type)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("O nome não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("O e-mail não pode ser vazio.");

        if (!EmailValidator.IsValidEmail(email))
            throw new DomainException("O e-mail deve estar num formato válido.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("A senha não pode estar vazia.");

        if (type == EUserType.Common && !CpfValidator.IsValid(document))
            throw new InvalidDocumentException("O CPF deve estar num formato válido.");
        
        if (type == EUserType.Shopkeeper && !CnpjValidator.IsValid(document))
            throw new InvalidDocumentException("O CNPJ deve estar num formato válido.");
        
        return new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = passwordHash,
            Document = NormalizeDocument(document),
            Type = type,
            Balance = Math.Round(DomainConfiguration.NewUserBonus, 2), 
            TransactionsSent = new(),
            TransactionsReceived = new(),
        };
    }

    /// <summary>
    /// Realiza uma ação de débito no saldo do usuário
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ShopkeeperCannotTransferException"></exception>
    /// <exception cref="InvalidTransactionValueException"></exception>
    /// <exception cref="InsufficientBalanceException"></exception>
    public void Debit(decimal value)
    {
        if (Type == EUserType.Shopkeeper)
            throw new ShopkeeperCannotTransferException($"Apenas usuários do tipo {nameof(EUserType.Common)} podem realizar débitos.");
        
        switch (value)
        {
            case < DomainConfiguration.MinTransactionValue:
                throw new InvalidTransactionValueException($"O valor deve ser maior ou igual a {DomainConfiguration.MinTransactionValue}.");
            case > DomainConfiguration.MaxTransactionValue:
                throw new InvalidTransactionValueException($"O valor deve ser menor ou igual a {DomainConfiguration.MaxTransactionValue}.");
        }

        if (Balance < value)
            throw new InsufficientBalanceException("Saldo insuficiente para realizar a transação.");
        
        Balance -= value;
        UpdateDateModified();
    }
    
    /// <summary>
    /// Credita um valor ao saldo do usuário
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InvalidTransactionValueException"></exception>
    public void Credit(decimal value)
    {
        switch (value)
        {
            case < DomainConfiguration.MinTransactionValue:
                throw new InvalidTransactionValueException($"O valor deve ser maior ou igual a {DomainConfiguration.MinTransactionValue}.");
            case > DomainConfiguration.MaxTransactionValue:
                throw new InvalidTransactionValueException($"O valor deve ser menor ou igual a {DomainConfiguration.MaxTransactionValue}.");
            default:
                Balance += value;
                UpdateDateModified();
                break;
        }
    }

    /// <summary>
    /// Atualiza Nome e/ou E-mail do usuário
    /// </summary>
    /// <param name="fullName"></param>
    /// <param name="email"></param>
    public void UpdatePersonalInfo(string fullName, string email)
    {
        FullName = fullName;
        Email = email;   
        UpdateDateModified();
    }

    public static string NormalizeDocument(string document)
    {
        return new string(document.Where(char.IsLetterOrDigit).ToArray());
    }
}
