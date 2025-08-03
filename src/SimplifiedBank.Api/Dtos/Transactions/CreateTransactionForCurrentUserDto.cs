namespace SimplifiedBank.Api.Dtos.Transactions;

public sealed record CreateTransactionForCurrentUserDto
{
    public Guid ReceiverId { get; init; }
    public decimal Value { get; init; }
}