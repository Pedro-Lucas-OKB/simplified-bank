namespace SimplifiedBank.Domain;

public class DomainConfiguration
{
    public const decimal NewUserBonus = 100;  
    
    public const int UserFullNameMinimumLength = 3;
    public const int UserFullNameMaximumLength = 150;
    public const int UserPasswordMaximumLength = 100;

    public const decimal MaxTransactionValue = 10000000M;
    public const decimal MinTransactionValue = 0M;
}

