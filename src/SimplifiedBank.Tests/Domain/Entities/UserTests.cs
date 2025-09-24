using SimplifiedBank.Domain;
using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Enums;
using SimplifiedBank.Domain.Exceptions;

namespace SimplifiedBank.Tests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Create_CommonWithValidData_ShouldCreateUser()
    {
        // Arrange
        string fullName = "Pedro Lucas";
        string email = "pedrolucas@email.com";
        string passwordHash = "HashedPassword123!";
        string document = "354.652.910-36"; // Random CPF
        var type = EUserType.Common;

        // Act
        var user = User.Create(fullName, email, passwordHash, document, type);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(fullName, user.FullName);
        Assert.Equal(email, user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal(User.NormalizeDocument(document), user.Document);
        Assert.Equal(type, user.Type);
        Assert.Equal(DomainConfiguration.NewUserBonus, user.Balance);
    }

    [Fact]
    public void Create_ShopkeeperWithValidData_ShouldCreateUser()
    {
        // Arrange
        string fullName = "Pedro Lucas LTDA";
        string email = "pedrolucas@corp.com";
        string passwordHash = "HashedPassword123!";
        string document = "A8.BHT.FVP/0001-96"; // Random CNPJ
        var type = EUserType.Shopkeeper;

        // Act
        var user = User.Create(fullName, email, passwordHash, document, type);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(fullName, user.FullName);
        Assert.Equal(email, user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal(User.NormalizeDocument(document), user.Document);
        Assert.Equal(type, user.Type);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidName_ShouldThrowDomainException(string invalidName)
    {
        // Arrange
        string email = "pedrolucas@email.com";
        string passwordHash = "HashedPassword123!";
        string document = "354.652.910-36"; // Random CPF
        var type = EUserType.Common;

        // Act
        var exception =
            Assert.Throws<DomainException>(() => User.Create(invalidName, email, passwordHash, document, type));

        // Assert
        Assert.Equal("O nome não pode ser vazio.", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithEmptyEmail_ShouldThrowDomainException(string invalidEmail)
    {
        // Arrange
        string fullName = "Pedro Lucas";
        string passwordHash = "HashedPassword123!";
        string document = "354.652.910-36"; // Random CPF
        var type = EUserType.Common;

        // Act
        var exception =
            Assert.Throws<DomainException>(() => User.Create(fullName, invalidEmail, passwordHash, document, type));

        // Assert
        Assert.Equal("O e-mail não pode ser vazio.", exception.Message);
    }

    [Theory]
    [InlineData("pedrolucas@email")]
    [InlineData("pedrolucas@email.")]
    [InlineData("pedrolucasemail.com")]
    [InlineData("@email.com")]
    public void Create_WithInvalidEmail_ShouldThrowDomainException(string invalidEmail)
    {
        // Arrange
        string fullName = "Pedro Lucas";
        string passwordHash = "HashedPassword123!";
        string document = "354.652.910-36"; // Random CPF
        var type = EUserType.Common;

        // Act
        var exception =
            Assert.Throws<DomainException>(() => User.Create(fullName, invalidEmail, passwordHash, document, type));

        // Assert
        Assert.Equal("O e-mail deve estar num formato válido.", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithEmptyPassword_ShouldThrowDomainException(string invalidPassword)
    {
        // Arrange
        string fullName = "Pedro Lucas";
        string email = "pedrolucas@email.com";
        string document = "354.652.910-36"; // Random CPF
        var type = EUserType.Common;

        // Act
        var exception =
            Assert.Throws<DomainException>(() => User.Create(fullName, email, invalidPassword, document, type));

        // Assert
        Assert.Equal("A senha não pode estar vazia.", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("12345678901")]
    [InlineData("354.652.910-30")]
    [InlineData("354.652.980-36")]
    [InlineData("35a.6f2.910-36")]
    [InlineData("354.652.910-96")]
    [InlineData("123456789101112")]
    public void Create_WithInvalidCpf_ShouldThrowInvalidDocumentException(string invalidCpf)
    {
        // Arrange
        string fullName = "Pedro Lucas";
        string email = "pedrolucas@email.com";
        string passwordHash = "HashedPassword123!";
        var type = EUserType.Common;

        // Act
        var exception =
            Assert.Throws<InvalidDocumentException>(() => User.Create(fullName, email, passwordHash, invalidCpf, type));

        // Assert
        Assert.Equal("O CPF deve estar num formato válido.", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("12345678901")]
    [InlineData("354.652.910-30")]
    [InlineData("354.652.910-96")]
    [InlineData("A8.BHT.FVP/0001-66")]
    [InlineData("A8.BHT.FVP/0001-99")]
    [InlineData("123456789101112121415")]
    public void Create_WithInvalidCnpj_ShouldThrowInvalidDocumentException(string invalidCnpj)
    {
        // Arrange
        string fullName = "Pedro Lucas LTDA";
        string email = "pedrolucas@corp.com";
        string passwordHash = "HashedPassword123!";
        var type = EUserType.Shopkeeper;

        // Act
        var exception =
            Assert.Throws<InvalidDocumentException>(() =>
                User.Create(fullName, email, passwordHash, invalidCnpj, type));

        // Assert
        Assert.Equal("O CNPJ deve estar num formato válido.", exception.Message);
    }

    [Fact]
    public void Debit_WithValidValue_ShouldDebitUser()
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        decimal debitValue = 50M;
        decimal expectedBalance = user.Balance - debitValue;
        
        // Act
        user.Debit(debitValue);
        
        // Assert
        Assert.Equal(expectedBalance, user.Balance);
    }
    
    [Theory]
    [InlineData("0d")]
    [InlineData("0.0d")]
    [InlineData("test")]
    public void Debit_WithInvalidDecimal_ShouldThrowFormatException(string invalidValue)
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        // Act
        var exception = Assert.Throws<FormatException>(() => user.Debit(decimal.Parse(invalidValue)));
        
        // Assert
        Assert.Equal(typeof(FormatException), exception.GetType());   
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1.5)]
    [InlineData(-0.00000001)]
    public void Debit_WithNegativeOrZeroValue_ShouldThrowInvalidTransactionValueException(decimal invalidValue)
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        // Act
        var exception = Assert.Throws<InvalidTransactionValueException>(() => user.Debit(invalidValue));
        
        // Assert
        Assert.Equal($"O valor não deve ser negativo ou zero.", exception.Message);          
    }

    [Fact]
    public void Debit_WithSmallValue_ShouldThrowInvalidTransactionValueException()
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        var invalidValue = DomainConfiguration.MinTransactionValue - 0.00000001M;
        
        // Act
        var exception = Assert.Throws<InvalidTransactionValueException>(() => user.Debit(invalidValue));
        
        // Assert
        Assert.Equal($"O valor deve ser maior ou igual a {DomainConfiguration.MinTransactionValue}.", exception.Message);          
    }
    
    [Fact]
    public void Debit_WithHugeValue_ShouldThrowInvalidTransactionValueException()
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        var invalidValue = DomainConfiguration.MaxTransactionValue + 0.00000001M;
        
        // Act
        var exception = Assert.Throws<InvalidTransactionValueException>(() => user.Debit(invalidValue));
        
        // Assert
        Assert.Equal($"O valor deve ser menor ou igual a {DomainConfiguration.MaxTransactionValue}.", exception.Message);          
    }
    
    [Fact]
    public void Debit_WithNotEnoughBalance_ShouldThrowInsufficientBalanceException()
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        var invalidValue = DomainConfiguration.NewUserBonus + 0.00000001M;
        
        // Act
        var exception = Assert.Throws<InsufficientBalanceException>(() => user.Debit(invalidValue));
        
        // Assert
        Assert.Equal("Saldo insuficiente para realizar a transação.", exception.Message);          
    }
    
    
    [Fact]
    public void Credit_WithValidValue_ShouldCreditUser()
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        decimal creditValue = 50M;
        decimal expectedBalance = user.Balance + creditValue;
        
        // Act
        user.Credit(creditValue);
        
        // Assert
        Assert.Equal(expectedBalance, user.Balance);
    }
    
    [Theory]
    [InlineData("0d")]
    [InlineData("0.0d")]
    [InlineData("test")]
    public void Credit_WithInvalidDecimal_ShouldThrowFormatException(string invalidValue)
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        // Act
        var exception = Assert.Throws<FormatException>(() => user.Credit(decimal.Parse(invalidValue)));
        
        // Assert
        Assert.Equal(typeof(FormatException), exception.GetType());   
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1.5)]
    [InlineData(-0.00000001)]
    public void Credit_WithNegativeOrZeroValue_ShouldThrowInvalidTransactionValueException(decimal invalidValue)
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        // Act
        var exception = Assert.Throws<InvalidTransactionValueException>(() => user.Credit(invalidValue));
        
        // Assert
        Assert.Equal($"O valor não deve ser negativo ou zero.", exception.Message);          
    }

    [Fact]
    public void Credit_WithSmallValue_ShouldThrowInvalidTransactionValueException()
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        var invalidValue = DomainConfiguration.MinTransactionValue - 0.00000001M;
        
        // Act
        var exception = Assert.Throws<InvalidTransactionValueException>(() => user.Credit(invalidValue));
        
        // Assert
        Assert.Equal($"O valor deve ser maior ou igual a {DomainConfiguration.MinTransactionValue}.", exception.Message);          
    }
    
    [Fact]
    public void Credit_WithHugeValue_ShouldThrowInvalidTransactionValueException()
    {
        // Arrange
        var user = User.Create(
            "Pedro Lucas",
            "pedrolucas@email.com",
            "HashedPassword123!",
            "354.652.910-36", // Random CPF
            EUserType.Common);
        
        var invalidValue = DomainConfiguration.MaxTransactionValue + 0.00000001M;
        
        // Act
        var exception = Assert.Throws<InvalidTransactionValueException>(() => user.Credit(invalidValue));
        
        // Assert
        Assert.Equal($"O valor deve ser menor ou igual a {DomainConfiguration.MaxTransactionValue}.", exception.Message);          
    }
}