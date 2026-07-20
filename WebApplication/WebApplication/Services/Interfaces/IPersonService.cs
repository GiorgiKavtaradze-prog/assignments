using WebApplication.DTOs;

namespace WebApplication.Services.Interfaces;

public interface IPersonService
{
    Task<PagedResult<PersonDto>> GetPagedAsync(PersonFilter filter, CancellationToken cancellationToken);

    Task<PersonDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<PersonDto>> FilterAsync(PersonFilter filter, CancellationToken cancellationToken);

    Task<PersonDto> AddAsync(PersonCreateDto dto, CancellationToken cancellationToken);

    Task<PersonDto?> UpdateAsync(int id, PersonUpdateDto dto, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
