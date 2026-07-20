namespace WebApplication.DTOs;

public sealed record PersonFilter
{
    public decimal? MinSalary { get; init; }

    public decimal? MaxSalary { get; init; }

    public string? City { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public bool IsFiltering =>
        MinSalary.HasValue || MaxSalary.HasValue || !string.IsNullOrWhiteSpace(City);
}
