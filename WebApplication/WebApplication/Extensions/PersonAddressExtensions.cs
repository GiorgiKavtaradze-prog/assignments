using WebApplication.DTOs;
using WebApplication.Models;

namespace WebApplication.Extensions;

public static class PersonAddressExtensions
{
    public static PersonAddressDto ToDto(this PersonAddress address)
    {
        return new PersonAddressDto
        {
            Country = address.Country,
            City = address.City,
            HomeNumber = address.HomeNumber
        };
    }

    public static PersonAddress ToEntity(this PersonAddressCreateDto dto)
    {
        return new PersonAddress
        {
            Country = dto.Country,
            City = dto.City,
            HomeNumber = dto.HomeNumber
        };
    }
}