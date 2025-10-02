using FluentAssertions;
using SimplifiedBank.Domain.Validators;

namespace SimplifiedBank.Tests.Domain.Validators;

public class EmailValidatorTests
{
    [Theory]
    [InlineData("pedro@email.com")]
    [InlineData("pedro.lucas@email.com")]
    [InlineData("pedro.lucas@email.com.br")]
    [InlineData("pedro_lucas@email.com")]
    [InlineData("pedro123@email.com")]
    [InlineData("pedro+lucas@email.com")]
    public void IsValidEmail_WithValidEmails_ShouldReturnTrue(string email)
    {
        // Act
        var result = EmailValidator.IsValidEmail(email);
        
        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("pedro@email.com    ")]
    [InlineData("    pedro@email.com")]
    [InlineData("   pedro@email.com   ")]
    public void IsValidEmail_WithWhiteSpaces_ShouldReturnTrue(string email)
    {
        // Act
        var result = EmailValidator.IsValidEmail(email);
        
        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("pedro")] // Sem @ e sem ponto
    [InlineData("@email.com")] // Sem local-part
    [InlineData("pedro@")] // Sem domínio
    [InlineData("pedro@email")] // Sem TLD
    [InlineData("pedro@.com")] // Sem domínio
    [InlineData(".pedro@email.com")] // Começa com ponto
    [InlineData("pedro.@email.com")] // Termina com ponto antes do @
    [InlineData("pedro@email.")] // Termina com ponto
    [InlineData("pedro@@email.com")] // @ duplicado
    [InlineData("pedro@email..com")] // Pontos consecutivos
    public void IsValidEmail_WithInvalidFormat_ShouldReturnFalse(string email)
    {
        // Act
        var result = EmailValidator.IsValidEmail(email);
        
        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("")] // Vazio
    [InlineData(" ")] // Espaço
    [InlineData("    ")] // Múltiplos espaços
    [InlineData(null)] // Null
    public void IsValidEmail_WithNullOrEmpty_ShouldReturnFalse(string email)
    {
        // Act
        var result = EmailValidator.IsValidEmail(email);
        
        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("pedro lucas@email.com")] // Com espaço
    [InlineData("pedro\tlucas@email.com")] // Com tab
    [InlineData("pedro\nlucas@email.com")] // Com quebra de linha
    public void IsValidEmail_WithWhiteSpacesInLocalPart_ShouldReturnFalse(string email)
    {
        // Act
        var result = EmailValidator.IsValidEmail(email);
        
        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidEmail_WithVeryLongEmail_ShouldReturnFalse()
    {
        // Arrange
        var longLocalPart = new string('a', 65); // RFC 5321 limite de 64 caracteres
        var longDomain = new string('a', 256);
        var email = $"{longLocalPart}@{longDomain}.com";

        // Act
        var result = EmailValidator.IsValidEmail(email);
        
        // Assert
        result.Should().BeFalse();
    }
}