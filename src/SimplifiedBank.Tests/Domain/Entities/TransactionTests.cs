using System.Runtime.Intrinsics.Arm;
using SimplifiedBank.Domain;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Exceptions;

namespace SimplifiedBank.Tests.Domain.Entities;

public class TransactionTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateTransaction()
    {
        // Arrange
        var senderId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        const decimal value = 100.00M;
        
        // Act
        var transaction = Transaction.Create(senderId, receiverId, value);
        
        // Assert
        Assert.NotNull(transaction);
        Assert.Equal(senderId, transaction.SenderId);
        Assert.Equal(receiverId, transaction.ReceiverId);
        Assert.Equal(value, transaction.Value);       
    }
    
    [Fact]
    public void Create_WithEmptySenderGuid_ShouldThrowDomainException()
    {
        // Arrange
        var senderId = Guid.Empty;
        var receiverId = Guid.NewGuid();
        const decimal value = 100.00M;
        
        // Act
        var exception = Assert.Throws<DomainException>(() => Transaction.Create(senderId, receiverId, value));
        
        // Assert
        Assert.Equal("ID de pagador é inválido.", exception.Message);
    }
    
    [Fact]
    public void Create_WithEmptyReceiverGuid_ShouldThrowDomainException()
    {
        // Arrange
        var senderId = Guid.NewGuid();
        var receiverId = Guid.Empty;
        const decimal value = 100.00M;
        
        // Act
        var exception = Assert.Throws<DomainException>(() => Transaction.Create(senderId, receiverId, value));
        
        // Assert
        Assert.Equal("ID de recebedor é inválido.", exception.Message);
    }
    
    [Fact]
    public void Create_WithSameSenderAndReceiverGuid_ShouldThrowSenderAndReceiverCannotBeEqualException()
    {
        // Arrange
        var senderId = Guid.NewGuid();
        var receiverId = senderId;
        const decimal value = 100.00M;
        
        // Act
        var exception = Assert.Throws<SenderAndReceiverCannotBeEqualException>(() => Transaction.Create(senderId, receiverId, value));
        
        // Assert
        Assert.Equal("Um usuário não pode realizar uma transação para si mesmo.", exception.Message);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1.5)]
    public void Create_WithNegativeOrZeroValue_ShouldThrowInvalidTransactionValueException(decimal invalidValue)
    {
        // Arrange
        var senderId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        
        // Act
        var exception = Assert.Throws<InvalidTransactionValueException>(() => Transaction.Create(senderId, receiverId, invalidValue));
        
        // Assert
        Assert.Equal("O valor não deve ser negativo ou zero.", exception.Message);
    }
    
    [Fact]
    public void Create_WithSmallValue_ShouldThrowInvalidTransactionValueException()
    {
        // Arrange
        var senderId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        const decimal value = DomainConfiguration.MinTransactionValue - 0.00000001M;
        
        // Act
        var exception = Assert.Throws<InvalidTransactionValueException>(() => Transaction.Create(senderId, receiverId, value));
        
        // Assert
        Assert.Equal($"O valor deve ser maior ou igual a {DomainConfiguration.MinTransactionValue}.", exception.Message);
    }
    
    [Fact]
    public void Create_WithHugeValue_ShouldThrowInvalidTransactionValueException()
    {
        // Arrange
        var senderId = Guid.NewGuid();
        var receiverId = Guid.NewGuid();
        const decimal value = DomainConfiguration.MaxTransactionValue + 0.00000001M;
        
        // Act
        var exception = Assert.Throws<InvalidTransactionValueException>(() => Transaction.Create(senderId, receiverId, value));
        
        // Assert
        Assert.Equal($"O valor deve ser menor ou igual a {DomainConfiguration.MaxTransactionValue}.", exception.Message);
    }
}