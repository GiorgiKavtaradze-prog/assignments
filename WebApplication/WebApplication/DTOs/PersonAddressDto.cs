namespace WebApplication.DTOs;

public sealed record PersonAddressDto
{
    public required string Country { get; init; }

    public required string City { get; init; }

    public required string HomeNumber { get; init; }
}
