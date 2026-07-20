namespace WebApplication.DTOs;

public sealed record PersonDto
{
    public int Id { get; init; }

    public DateTime CreateDate { get; init; }

    public required string Firstname { get; init; }

    public required string Lastname { get; init; }

    public required string JobPosition { get; init; }

    public decimal Salary { get; init; }

    public double WorkExperience { get; init; }

    public required PersonAddressDto Address { get; init; }
}
