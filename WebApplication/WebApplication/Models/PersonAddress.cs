namespace WebApplication.Models;

public sealed record PersonAddress
{
    public required string Country { get; init; }

    public required string City { get; init; }

    public required string HomeNumber { get; init; }
}
