using AutoMapper;
using WebApplication.Dtos;
using WebApplication.Models;

namespace WebApplication.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonDto>();
        CreateMap<PersonDto, Person>();
        CreateMap<PersonCreateDto, Person>();
        CreateMap<PersonUpdateDto, Person>();
        CreateMap<PersonAddress, PersonAddressDto>();
        CreateMap<PersonAddressDto, PersonAddress>();
        CreateMap<PersonAddressCreateDto, PersonAddress>();
    }
}