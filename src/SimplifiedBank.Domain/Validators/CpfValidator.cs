namespace SimplifiedBank.Domain.Validators;

public abstract class CpfValidator
{
    public static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) 
            return false;
        
        cpf = new string(cpf.Where(char.IsDigit).ToArray());
        
        if (cpf.All(x => x == cpf[0]))
            return false;
        
        if (cpf.Length != 11) 
            return false;

        int digit1 = short.Parse(cpf[9].ToString());
        int digit2 = short.Parse(cpf[10].ToString());

        int sum = 0;
        int multiplier = 10;
        
        for (int i = 0; i < 9; i++)
        {
            sum += (short.Parse(cpf[i].ToString()) * multiplier);
            multiplier--;
        }

        int rest = sum % 11;

        if (rest < 2 && digit1 != 0)
            return false;
        else if (rest >= 2 && (11 - rest != digit1))
            return false;
        
        multiplier = 11;
        sum = 0;
        
        for (int i = 0; i < 10; i++)
        {
            sum += (short.Parse(cpf[i].ToString()) * multiplier);
            multiplier--;
        }
        
        rest = sum % 11;
        
        if (rest < 2 && digit2 != 0)
            return false;
        else if (rest >= 2 && (11 - rest != digit2))
            return false;
        
        return true;
    }
}