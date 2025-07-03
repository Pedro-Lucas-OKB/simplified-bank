using SimplifiedBank.Application.Shared.Responses;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Application.UseCases.Users.GetUserTransactions;

public sealed record GetUserTransactionsResponse : UserResponse
{
    public List<UserTransactionResponse> Transactions { get; init; } = new();

    public static implicit operator GetUserTransactionsResponse(User user)
        => new()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Document = user.Document,
            Balance = user.Balance,
            Type = user.Type,
            Transactions = UserTransactionResponse.ConvertAll(user.TransactionsSent ?? user.TransactionsReceived)
        };
}