using FluentAssertions;
using SimplifiedBank.Domain.Validators;

namespace SimplifiedBank.Tests.Domain.Validators;

public class CnpjValidatorTests
{
    [Theory]
    [InlineData("A8.BHT.FVP/0001-96")] // Alfanumérico
    [InlineData("12.345.678/0001-95")] // Numérico tradicional
    [InlineData("00.000.000/0001-91")]
    [InlineData("A8BHTFVP000196")] // Sem formatação
    [InlineData("12345678000195")] // Sem formatação
    public void IsValid_WithValidCnpj_ShouldReturnTrue(string cnpj)
    {
        // Act
        var result = CnpjValidator.IsValid(cnpj);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void IsValid_WithNullOrEmptyOrWhiteSpace_ShouldReturnFalse(string cnpj)
    {
        // Act
        var result = CnpjValidator.IsValid(cnpj);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("AA.AAA.AAA/0001-AA")] // Letras repetidas
    [InlineData("11.111.111/1111-11")] // Números repetidos
    [InlineData("00.000.000/0000-00")]
    [InlineData("ZZ.ZZZ.ZZZ/ZZZZ-ZZ")]
    public void IsValid_WithRepeatedCharacters_ShouldReturnFalse(string cnpj)
    {
        // Act
        var result = CnpjValidator.IsValid(cnpj);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("A8.BHT.FVP/0001-9")] // Faltando um dígito
    [InlineData("A8.BHT.FVP/0001-965")] // Com um dígito extra
    [InlineData("A8.BHT.FVP/001-96")] // Faltando um dígito no meio
    [InlineData("A8BHTFVP00019")] // Faltando um dígito sem formatação
    [InlineData("A8BHTFVP0001965")] // Com dígito extra sem formatação
    public void IsValid_WithInvalidLength_ShouldReturnFalse(string cnpj)
    {
        // Act
        var result = CnpjValidator.IsValid(cnpj);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("A8.BHT.FVP/0001-95")] // Último dígito errado
    [InlineData("A8.BHT.FVP/0001-86")] // Penúltimo dígito errado
    [InlineData("A8.BHT.FVP/0001-85")] // Ambos dígitos verificadores errados
    public void IsValid_WithInvalidCheckDigits_ShouldReturnFalse(string cnpj)
    {
        // Act
        var result = CnpjValidator.IsValid(cnpj);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("A8.BHT.FV$0001-96")]
    [InlineData("A8.BHT.FV#0001-96")]
    [InlineData("A8.BHT.FV@0001-96")]
    [InlineData("A8+BH+FVP/0001-96")]
    [InlineData("!@#$%¨&*()_+")]
    public void IsValid_WithInvalidSpecialCharacters_ShouldReturnFalse(string cnpj)
    {
        // Act
        var result = CnpjValidator.IsValid(cnpj);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("  A8.BHT.FVP/0001-96  ")] // Espaços no início e fim
    [InlineData("A8.BHT.FVP/0001-96  ")] // Espaços no fim
    [InlineData("  A8.BHT.FVP/0001-96")] // Espaços no início
    public void IsValid_WithLeadingAndTrailingSpaces_ShouldReturnTrue(string cnpj)
    {
        // Act
        var result = CnpjValidator.IsValid(cnpj);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("a8.bht.fvp/0001-96")] // Letras minúsculas
    [InlineData("A8.BHT.FVP/0001-96")] // Letras maiúsculas
    [InlineData("a8.BHt.FvP/0001-96")] // Misturado
    public void IsValid_WithMixedCase_ShouldReturnTrue(string cnpj)
    {
        // Act
        var result = CnpjValidator.IsValid(cnpj);

        // Assert
        result.Should().BeTrue();
    }
}