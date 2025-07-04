using SimplifiedBank.Domain.Entities;
using SimplifiedBank.Domain.Enums;

namespace SimplifiedBank.Application.Shared.Responses;

public record UserResponse
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Document { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    public EUserType Type { get; init; }
    public string TypeName => Type.ToString();

    /// <summary>
    /// Mapeamento nativo de User para UserResponse
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static implicit operator UserResponse(User user)
        => new()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Document = user.Document,
            Balance = user.Balance,
            Type = user.Type
        };
    
    /// <summary>
    /// Converte uma lista de Users em uma lista de UserResponse
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public static List<UserResponse> ConvertAll(List<User> users)
    {
        List<UserResponse> userResponses = new();

        foreach (var user in users)
        {
            userResponses.Add(user);
        }
        
        return userResponses;
    }
}
