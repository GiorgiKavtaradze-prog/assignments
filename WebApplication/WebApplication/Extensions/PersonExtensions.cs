using WebApplication.DTOs;
using WebApplication.Models;

namespace WebApplication.Extensions;

public static class PersonExtensions
{
    public static PersonDto ToDto(this Person person)
    {
        return new PersonDto
        {
            Id = person.Id,
            CreateDate = person.CreateDate,
            Firstname = person.Firstname,
            Lastname = person.Lastname,
            JobPosition = person.JobPosition,
            Salary = person.Salary,
            WorkExperience = person.WorkExperience,
            Address = person.Address.ToDto()
        };
    }

    public static IReadOnlyList<PersonDto> ToDtos(this IEnumerable<Person> people)
    {
        return people.Select(p => p.ToDto()).ToList();
    }

    public static Person ToEntity(this PersonCreateDto dto, int id)
    {
        return new Person
        {
            Id = id,
            CreateDate = dto.CreateDate,
            Firstname = dto.Firstname,
            Lastname = dto.Lastname,
            JobPosition = dto.JobPosition,
            Salary = dto.Salary,
            WorkExperience = dto.WorkExperience,
            Address = dto.Address.ToEntity()
        };
    }

    public static Person With(this Person person, PersonUpdateDto dto)
    {
        return new Person
        {
            Id = person.Id,
            CreateDate = person.CreateDate,
            Firstname = dto.Firstname,
            Lastname = dto.Lastname,
            JobPosition = dto.JobPosition,
            Salary = dto.Salary,
            WorkExperience = dto.WorkExperience,
            Address = dto.Address.ToEntity()
        };
    }
}