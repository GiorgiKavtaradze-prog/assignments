using Microsoft.AspNetCore.Mvc;
using WebApplication.Dtos;
using WebApplication.Responses;
using WebApplication.Services;

namespace WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PersonDto>>> CreatePerson([FromBody] PersonCreateDto person)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<PersonDto>
            {
                Success = false,
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
            });
        }

        var createdPerson = await _personService.AddAsync(person);
        return CreatedAtAction(nameof(GetPersonById), new { id = createdPerson.Id }, new ApiResponse<PersonDto>
        {
            Success = true,
            Data = createdPerson,
            Message = "Person created successfully"
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<ApiResponse<List<PersonDto>>>> GetAllPeople(
        [FromQuery] double? minSalary,
        [FromQuery] double? maxSalary,
        [FromQuery] string? city)
    {
        List<PersonDto> people;
        
        if (minSalary.HasValue || maxSalary.HasValue || !string.IsNullOrWhiteSpace(city))
        {
            people = await _personService.FilterAsyncDto(minSalary, maxSalary, city);
        }
        else
        {
            people = await _personService.GetAllWithoutFilterAsyncDto();
        }
        
        return Ok(new ApiResponse<List<PersonDto>>
        {
            Success = true,
            Data = people
        });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PersonDto>>> GetPersonById(int id)
    {
        var person = await _personService.GetByIdAsyncDto(id);
        if (person == null)
        {
            return NotFound(new ApiResponse<PersonDto>
            {
                Success = false,
                Message = $"Person with id {id} not found"
            });
        }
        return Ok(new ApiResponse<PersonDto>
        {
            Success = true,
            Data = person
        });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<PersonDto>>> UpdatePerson(int id, [FromBody] PersonUpdateDto person)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<PersonDto>
            {
                Success = false,
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
            });
        }

        var updatedPerson = await _personService.UpdateAsync(id, person);
        if (updatedPerson == null)
        {
            return NotFound(new ApiResponse<PersonDto>
            {
                Success = false,
                Message = $"Person with id {id} not found"
            });
        }
        return Ok(new ApiResponse<PersonDto>
        {
            Success = true,
            Data = updatedPerson,
            Message = "Person updated successfully"
        });
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<List<PersonDto>>>> DeletePerson(int id)
    {
        var deleted = await _personService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<List<PersonDto>>
            {
                Success = false,
                Message = $"Person with id {id} not found"
            });
        }
        
        var updatedList = await _personService.GetAllWithoutFilterAsyncDto();
        return Ok(new ApiResponse<List<PersonDto>>
        {
            Success = true,
            Data = updatedList,
            Message = "Person deleted successfully"
        });
    }
}
