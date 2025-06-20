using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Enums;

namespace SimplifiedBank.Application.UseCases.Users.GetById;

public sealed record GetByIdResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public EUserType Type { get; set; }
    
    /// <summary>
    /// Mapeamento nativo de User para GetByIdResponse
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static implicit operator GetByIdResponse(User user)
    {
        return new GetByIdResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Document = user.Document,
            Balance = user.Balance,
            Type = user.Type
        };
    }
}
