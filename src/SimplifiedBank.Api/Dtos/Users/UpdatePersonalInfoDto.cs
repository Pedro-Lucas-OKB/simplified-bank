namespace SimplifiedBank.Api.Dtos.Users;

public sealed record UpdatePersonalInfoDto
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}