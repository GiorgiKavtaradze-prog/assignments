using FluentValidation;
using WebApplication.DTOs;

namespace WebApplication.Validators;

public sealed class PersonAddressCreateDtoValidator : AbstractValidator<PersonAddressCreateDto>
{
    public PersonAddressCreateDtoValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(50).WithMessage("Country must not exceed 50 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(50).WithMessage("City must not exceed 50 characters");

        RuleFor(x => x.HomeNumber)
            .NotEmpty().WithMessage("HomeNumber is required")
            .MaximumLength(50).WithMessage("HomeNumber must not exceed 50 characters");
    }
}
