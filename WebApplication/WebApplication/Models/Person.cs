namespace WebApplication.Models;

public sealed record Person
{
    public required int Id { get; init; }

    public DateTime CreateDate { get; init; } = DateTime.UtcNow;

    public required string Firstname { get; init; }

    public required string Lastname { get; init; }

    public required string JobPosition { get; init; }

    public required decimal Salary { get; init; }

    public required double WorkExperience { get; init; }

    public required PersonAddress Address { get; init; }
}
