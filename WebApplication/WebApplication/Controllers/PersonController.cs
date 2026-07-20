using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DTOs;
using WebApplication.Responses;
using WebApplication.Services.Interfaces;

namespace WebApplication.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public sealed class PersonController(IPersonService personService) : ControllerBase
{
    private readonly IPersonService _personService = personService;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PersonDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<PersonDto>>>> GetPeople(
        [FromQuery] PersonFilter filter,
        CancellationToken cancellationToken)
    {
        var paged = await _personService.GetPagedAsync(filter, cancellationToken);
        return Ok(ApiResponse<PagedResult<PersonDto>>.Ok(paged));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PersonDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PersonDto>>> GetPersonById(int id, CancellationToken cancellationToken)
    {
        var person = await _personService.GetByIdAsync(id, cancellationToken);
        if (person is null)
        {
            return NotFound(ApiResponse<PersonDto>.Fail($"Person with id {id} not found"));
        }

        return Ok(ApiResponse<PersonDto>.Ok(person));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PersonDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PersonDto>>> CreatePerson(
        [FromBody] PersonCreateDto person,
        CancellationToken cancellationToken)
    {
        var created = await _personService.AddAsync(person, cancellationToken);

        return CreatedAtAction(
            nameof(GetPersonById),
            new { id = created.Id },
            ApiResponse<PersonDto>.Ok(created, "Person created successfully"));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PersonDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PersonDto>>> UpdatePerson(
        int id,
        [FromBody] PersonUpdateDto person,
        CancellationToken cancellationToken)
    {
        var updated = await _personService.UpdateAsync(id, person, cancellationToken);
        if (updated is null)
        {
            return NotFound(ApiResponse<PersonDto>.Fail($"Person with id {id} not found"));
        }

        return Ok(ApiResponse<PersonDto>.Ok(updated, "Person updated successfully"));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePerson(int id, CancellationToken cancellationToken)
    {
        var deleted = await _personService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
