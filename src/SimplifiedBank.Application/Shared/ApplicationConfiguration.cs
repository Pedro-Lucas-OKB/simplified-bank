using System.Net;

namespace SimplifiedBank.Application.Shared;

public class ApplicationConfiguration
{
    public PaginationSettings PaginationSettings { get; set; } = new();
}

public class PaginationSettings
{
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 30;
    public const int MaxPageSize = 100;
}