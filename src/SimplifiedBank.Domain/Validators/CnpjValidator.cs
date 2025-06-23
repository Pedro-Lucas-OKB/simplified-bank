namespace SimplifiedBank.Domain.Validators;

public class CnpjValidator
{
    private static string PermitedLetters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public static bool IsValid(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj)) 
            return false;
        
        cnpj = new string(cnpj.Where(char.IsLetterOrDigit).ToArray());
        
        if (cnpj.All(x => x == cnpj[0]))
            return false;
        
        if (cnpj.Length != 14) 
            return false;
        
        if (!IsNumberDigits(cnpj))
            return false;
        
        var digit1 = CharToInt(cnpj[12]);
        var digit2 = CharToInt(cnpj[13]);
        
        // Verificações do primeiro digito
        short[] peso1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int sum = 0;
        
        for (int i = 0; i < peso1.Length; i++)
        {
            var digit = CharToInt(cnpj[i]);
            if (digit < 0)
                return false;
            
            sum += digit * peso1[i];
        }
        
        int rest = sum % 11;
        
        if (!ValidateDigit(rest, digit1)) 
            return false;
        
        // Verificações para o segundo digito
        short[] peso2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        sum = 0;
        
        for (int i = 0; i < peso2.Length; i++)
        {
            var digit = CharToInt(cnpj[i]);
            if (digit < 0)
                return false;
            
            sum += digit * peso2[i];
        }
        
        rest = sum % 11;
        
        if (!ValidateDigit(rest, digit2)) 
            return false;
        
        return true;
    }

    /// <summary>
    /// Verifica se o DV atende as regras
    /// </summary>
    /// <param name="rest"></param>
    /// <param name="digit"></param>
    /// <returns></returns>
    private static bool ValidateDigit(int rest, int digit)
    {
        if (rest < 2 && digit != 0)
            return false;
        else if (rest >= 2 && (11 - rest) != digit)
            return false;
        return true;
    }

    private static int CharToInt(char c)
    {
        if(char.IsDigit(c))
            return c - '0';
        else if (PermitedLetters.Contains(char.ToUpper(c)))
            return char.ToUpper(c) - '0';
        else
            return -1;
    }

    private static bool IsNumberDigits(string cnpj)
    {
        if (char.IsLetter(cnpj[12]) || char.IsLetter(cnpj[13]))
            return false;
        
        return true;
    }
}