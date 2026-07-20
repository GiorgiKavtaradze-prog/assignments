using System.Text.Json;
using WebApplication.DTOs;
using WebApplication.Extensions;
using WebApplication.Models;
using WebApplication.Services.Interfaces;

namespace WebApplication.Services;

public sealed class PersonService : IPersonService
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);
    private readonly ILogger<PersonService> _logger;

    public PersonService(string filePath, ILogger<PersonService> logger)
    {
        _filePath = filePath;
        _logger = logger;
        EnsureFileExists();
    }

    public async Task<PagedResult<PersonDto>> GetPagedAsync(PersonFilter filter, CancellationToken cancellationToken)
    {
        var people = await ReadAllAsync(cancellationToken);
        var filtered = ApplyFilter(people, filter);

        var page = Math.Max(filter.Page, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, 200);

        var pageItems = filtered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToDtos();

        return PagedResult<PersonDto>.Create(pageItems, page, pageSize, filtered.Count);
    }

    public async Task<PersonDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var people = await ReadAllAsync(cancellationToken);
        return people.FirstOrDefault(p => p.Id == id)?.ToDto();
    }

    public async Task<IReadOnlyList<PersonDto>> FilterAsync(PersonFilter filter, CancellationToken cancellationToken)
    {
        var people = await ReadAllAsync(cancellationToken);
        return ApplyFilter(people, filter).ToDtos();
    }

    public async Task<PersonDto> AddAsync(PersonCreateDto dto, CancellationToken cancellationToken)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            var people = await ReadAllUnsafeAsync(cancellationToken);
            var nextId = people.Count > 0 ? people.Max(p => p.Id) + 1 : 1;
            var person = dto.ToEntity(nextId);
            people.Add(person);
            await WriteAllAsync(people, cancellationToken);
            return person.ToDto();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<PersonDto?> UpdateAsync(int id, PersonUpdateDto dto, CancellationToken cancellationToken)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            var people = await ReadAllUnsafeAsync(cancellationToken);
            var index = people.FindIndex(p => p.Id == id);
            if (index == -1)
            {
                return null;
            }

            var updated = people[index].With(dto);
            people[index] = updated;
            await WriteAllAsync(people, cancellationToken);
            return updated.ToDto();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            var people = await ReadAllUnsafeAsync(cancellationToken);
            var index = people.FindIndex(p => p.Id == id);
            if (index == -1)
            {
                return false;
            }

            people.RemoveAt(index);
            await WriteAllAsync(people, cancellationToken);
            return true;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private static IReadOnlyList<Person> ApplyFilter(IReadOnlyList<Person> people, PersonFilter filter)
    {
        IEnumerable<Person> result = people;

        if (filter.MinSalary.HasValue)
        {
            result = result.Where(p => p.Salary >= filter.MinSalary.Value);
        }

        if (filter.MaxSalary.HasValue)
        {
            result = result.Where(p => p.Salary <= filter.MaxSalary.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            result = result.Where(p =>
                p.Address.City.Equals(filter.City, StringComparison.OrdinalIgnoreCase));
        }

        return result.ToArray();
    }

    private async Task<List<Person>> ReadAllAsync(CancellationToken ct)
    {
        await _fileLock.WaitAsync(ct);
        try
        {
            return await ReadAllUnsafeAsync(ct);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task<List<Person>> ReadAllUnsafeAsync(CancellationToken ct)
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        await using var stream = new FileStream(
            _filePath, FileMode.Open, FileAccess.Read, FileShare.Read,
            bufferSize: 4096, useAsync: true);

        var people = await JsonSerializer.DeserializeAsync(stream, PersonStoreJsonContext.Default.ListPerson, ct);
        return people ?? [];
    }

    private async Task WriteAllAsync(List<Person> people, CancellationToken ct)
    {
        var directory = Path.GetDirectoryName(_filePath) ?? AppContext.BaseDirectory;
        Directory.CreateDirectory(directory);

        var tempPath = Path.Combine(directory,
            $"{Path.GetFileNameWithoutExtension(_filePath)}.{Guid.NewGuid():N}.tmp");

        await using (var stream = new FileStream(
                        tempPath, FileMode.Create, FileAccess.Write, FileShare.None,
                        bufferSize: 4096, useAsync: true))
        {
            await JsonSerializer.SerializeAsync(stream, people, PersonStoreJsonContext.Default.ListPerson, ct);
        }
        File.Move(tempPath, _filePath, overwrite: true);
    }

    private void EnsureFileExists()
    {
        try
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize person store at {FilePath}", _filePath);
            throw;
        }
    }
}
