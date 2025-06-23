namespace SimplifiedBank.Application.Shared.Requests;

public abstract record PagedRequest
{
    public int PageSize { get; set; } = PaginationSettings.DefaultPageSize;
    public int PageNumber { get; set; } = PaginationSettings.DefaultPageNumber;
}