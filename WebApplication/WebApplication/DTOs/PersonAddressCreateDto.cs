namespace WebApplication.DTOs;

public sealed record PersonAddressCreateDto
{
    public required string Country { get; init; }

    public required string City { get; init; }

    public required string HomeNumber { get; init; }
}
