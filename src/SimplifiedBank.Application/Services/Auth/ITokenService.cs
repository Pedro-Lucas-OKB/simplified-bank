using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.Services.Auth;

public interface ITokenService
{
    public string GenerateToken(User user);
}