using AutoMapper;
using System.Text.Json;
using WebApplication.Dtos;
using WebApplication.Models;

namespace WebApplication.Services;

public class PersonService : IPersonService
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "people.json");
    private readonly IMapper _mapper;

    public PersonService(IMapper mapper)
    {
        _mapper = mapper;
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<List<Person>> GetAllAsync()
    {
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Person>>(json) ?? new List<Person>();
    }

    public async Task<PagedResult<PersonDto>> GetAllAsync(int page, int pageSize)
    {
        var people = await GetAllAsync();
        var totalPages = (int)Math.Ceiling((double)people.Count / pageSize);
        
        var pagedPeople = people
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<PersonDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = people.Count,
            TotalPages = totalPages,
            Data = _mapper.Map<List<PersonDto>>(pagedPeople)
        };
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        var people = await GetAllAsync();
        return people.FirstOrDefault(p => p.Id == id);
    }

    public async Task<PersonDto?> GetByIdAsyncDto(int id)
    {
        var person = await GetByIdAsync(id);
        return person != null ? _mapper.Map<PersonDto>(person) : null;
    }

    public async Task<Person> AddAsync(Person person)
    {
        var people = await GetAllAsync();
        person.Id = people.Count > 0 ? people.Max(p => p.Id) + 1 : 1;
        people.Add(person);
        await SaveToFileAsync(people);
        return person;
    }

    public async Task<PersonDto> AddAsync(PersonCreateDto personDto)
    {
        var person = _mapper.Map<Person>(personDto);
        var createdPerson = await AddAsync(person);
        return _mapper.Map<PersonDto>(createdPerson);
    }

    public async Task<Person?> UpdateAsync(int id, Person person)
    {
        var people = await GetAllAsync();
        var index = people.FindIndex(p => p.Id == id);
        if (index != -1)
        {
            person.Id = id;
            people[index] = person;
            await SaveToFileAsync(people);
            return person;
        }
        return null;
    }

    public async Task<PersonDto?> UpdateAsync(int id, PersonUpdateDto personDto)
    {
        var people = await GetAllAsync();
        var index = people.FindIndex(p => p.Id == id);
        if (index != -1)
        {
            var person = _mapper.Map<Person>(personDto);
            person.Id = id;
            people[index] = person;
            await SaveToFileAsync(people);
            return _mapper.Map<PersonDto>(person);
        }
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var people = await GetAllAsync();
        var personToRemove = people.FirstOrDefault(p => p.Id == id);
        if (personToRemove != null)
        {
            people.Remove(personToRemove);
            await SaveToFileAsync(people);
            return true;
        }
        return false;
    }

    public async Task<List<Person>> FilterAsync(double? minSalary, double? maxSalary, string? city)
    {
        var people = await GetAllAsync();
        var filtered = people.AsEnumerable();

        if (minSalary.HasValue)
        {
            filtered = filtered.Where(p => p.Salary >= minSalary.Value);
        }

        if (maxSalary.HasValue)
        {
            filtered = filtered.Where(p => p.Salary <= maxSalary.Value);
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            filtered = filtered.Where(p => p.Address.City.Equals(city, StringComparison.OrdinalIgnoreCase));
        }

        return filtered.ToList();
    }

    public async Task<List<Person>> GetAllWithoutFilterAsync()
    {
        return await GetAllAsync();
    }

    public async Task<List<PersonDto>> GetAllWithoutFilterAsyncDto()
    {
        var people = await GetAllWithoutFilterAsync();
        return _mapper.Map<List<PersonDto>>(people);
    }

    public async Task<List<PersonDto>> FilterAsyncDto(double? minSalary, double? maxSalary, string? city)
    {
        var people = await FilterAsync(minSalary, maxSalary, city);
        return _mapper.Map<List<PersonDto>>(people);
    }

    private async Task SaveToFileAsync(List<Person> people)
    {
        var json = JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json);
    }
}