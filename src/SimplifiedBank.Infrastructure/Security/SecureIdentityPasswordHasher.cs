using SecureIdentity.Password;
using SimplifiedBank.Domain.Interfaces;

namespace SimplifiedBank.Infrastructure.Security;

public class SecureIdentityPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => PasswordHasher.Hash(password);

    public bool Verify(string password, string hashedPassword) => PasswordHasher.Verify(hashedPassword, password);
}