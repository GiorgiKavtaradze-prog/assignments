namespace WebApplication.DTOs;

public sealed record PersonCreateDto
{
    public DateTime CreateDate { get; init; } = DateTime.UtcNow;

    public required string Firstname { get; init; }

    public required string Lastname { get; init; }

    public required string JobPosition { get; init; }

    public decimal Salary { get; init; }

    public double WorkExperience { get; init; }

    public required PersonAddressCreateDto Address { get; init; }
}
