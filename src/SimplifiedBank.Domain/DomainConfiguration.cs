namespace SimplifiedBank.Domain;

public class DomainConfiguration
{
    public const decimal NewUserBonus = 100;  
    
    public const int UserFullNameMinimumLength = 3;
    public const int UserFullNameMaximumLength = 150;
    public const int UserPasswordMaximumLength = 100;

    public const decimal MaxTransactionValue = 10000000M;
    public const decimal MinTransactionValue = 0M;

    public const string CommonUserFullNameRegexPattern =
        @"^(?:[A-ZÀ-Ý])(?:[']?[a-zà-ÿ])+(?:-(?:[A-ZÀ-Ý])(?:[']?[a-zà-ÿ])+)*(?:\s(?:(?:e|y|de(?:\s(?:la|las|lo|los))?|do|dos|da|das|del|van|von|bin|le)\s)?(?:(?:d'|D'|O'|Mc|Mac|al-)[A-ZÀ-Ý](?:[']?[a-zà-ÿ])+|[A-ZÀ-Ý](?:[']?[a-zà-ÿ])+(?:-(?:[A-ZÀ-Ý])(?:[']?[a-zà-ÿ])+)*))+(?:\s(?:Jr\.|II|III|IV))?$";
    
    public const string UserPasswordRegexPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*<>?&])[A-Za-z\d@$!%*<>?&]{8,}$";
}

