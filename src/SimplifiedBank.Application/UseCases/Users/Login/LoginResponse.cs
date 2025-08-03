namespace SimplifiedBank.Application.UseCases.Users.Login;

public sealed record LoginResponse
{
    public string Token { get; init; } = string.Empty;
    public bool IsSuccess { get; init; } = false;
    public string Message { get; init; } = string.Empty;
}