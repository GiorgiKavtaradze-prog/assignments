using WebApplication.Dtos;
using WebApplication.Models;

namespace WebApplication.Services;

public interface IPersonService
{
    Task<List<Person>> GetAllAsync();
    Task<PagedResult<PersonDto>> GetAllAsync(int page, int pageSize);
    Task<Person?> GetByIdAsync(int id);
    Task<PersonDto?> GetByIdAsyncDto(int id);
    Task<Person> AddAsync(Person person);
    Task<PersonDto> AddAsync(PersonCreateDto personDto);
    Task<Person?> UpdateAsync(int id, Person person);
    Task<PersonDto?> UpdateAsync(int id, PersonUpdateDto personDto);
    Task<bool> DeleteAsync(int id);
    Task<List<Person>> FilterAsync(double? minSalary, double? maxSalary, string? city);
    Task<List<PersonDto>> FilterAsyncDto(double? minSalary, double? maxSalary, string? city);
    Task<List<Person>> GetAllWithoutFilterAsync();
    Task<List<PersonDto>> GetAllWithoutFilterAsyncDto();
}
