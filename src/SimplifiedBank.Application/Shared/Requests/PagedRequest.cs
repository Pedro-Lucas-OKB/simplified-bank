namespace SimplifiedBank.Application.Shared.Requests;

public abstract record PagedRequest
{
    public int PageSize { get; init; } = PaginationSettings.DefaultPageSize;
    public int PageNumber { get; init; } = PaginationSettings.DefaultPageNumber;
}