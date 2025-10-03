using FluentAssertions;
using SimplifiedBank.Domain.Validators;

namespace SimplifiedBank.Tests.Domain.Validators;

public class CpfValidatorTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    [InlineData("145.382.206-20")]
    [InlineData("14538220620")]
    public void IsValid_WithValidCpf_ShouldReturnTrue(string cpf)
    {
        // Act
        var result = CpfValidator.IsValid(cpf);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void IsValid_WithNullOrEmptyOrWhiteSpace_ShouldReturnFalse(string cpf)
    {
        // Act
        var result = CpfValidator.IsValid(cpf);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("111.111.111-11")]
    [InlineData("222.222.222-22")]
    [InlineData("333.333.333-33")]
    [InlineData("444.444.444-44")]
    [InlineData("555.555.555-55")]
    [InlineData("666.666.666-66")]
    [InlineData("777.777.777-77")]
    [InlineData("888.888.888-88")]
    [InlineData("999.999.999-99")]
    [InlineData("000.000.000-00")]
    public void IsValid_WithRepeatedDigits_ShouldReturnFalse(string cpf)
    {
        // Act
        var result = CpfValidator.IsValid(cpf);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("529.982.247-2")] // Faltando um dígito
    [InlineData("529.982.247-255")] // Com um dígito extra
    [InlineData("529.982.24-25")] // Faltando um dígito no meio
    [InlineData("5299822472")] // Faltando um dígito sem pontuação
    [InlineData("529982247255")] // Com dígito extra sem pontuação
    public void IsValid_WithInvalidLength_ShouldReturnFalse(string cpf)
    {
        // Act
        var result = CpfValidator.IsValid(cpf);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("529.982.247-24")] // Último dígito errado
    [InlineData("529.982.247-15")] // Penúltimo dígito errado
    [InlineData("529.982.247-35")] // Ambos dígitos verificadores errados
    [InlineData("529.982.248-25")] // Um dígito do meio errado
    public void IsValid_WithInvalidCheckDigits_ShouldReturnFalse(string cpf)
    {
        // Act
        var result = CpfValidator.IsValid(cpf);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("5&9.%82.247@25")]
    [InlineData("529.982.247-2A")]
    [InlineData("529.9B2.247-25")]
    [InlineData("5 299 8 . 725A")]
    [InlineData("ABCDEFGHIJK")]
    public void IsValid_WithInvalidCharacters_ShouldReturnFalse(string cpf)
    {
        // Act
        var result = CpfValidator.IsValid(cpf);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithLeadingAndTrailingSpaces_ShouldReturnTrue()
    {
        // Arrange
        var cpf = "  529.982.247-25  ";

        // Act
        var result = CpfValidator.IsValid(cpf);

        // Assert
        result.Should().BeTrue();
    }
}