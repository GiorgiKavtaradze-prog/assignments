namespace WebApplication.DTOs;

public sealed record PagedResult<T>
{
    public required int Page { get; init; }

    public required int PageSize { get; init; }

    public required int TotalCount { get; init; }

    public required int TotalPages { get; init; }

    public required IReadOnlyList<T> Data { get; init; }

    public static PagedResult<T> Create(IReadOnlyList<T> data, int page, int pageSize, int totalCount)
    {
        var totalPages = pageSize > 0
            ? (int)Math.Ceiling((double)totalCount / pageSize)
            : 1;

        return new PagedResult<T>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Data = data
        };
    }
}
