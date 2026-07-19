using FluentValidation;
using WebApplication.Dtos;

namespace WebApplication.Validators;

public class PersonCreateDtoValidator : AbstractValidator<PersonCreateDto>
{
    public PersonCreateDtoValidator()
    {
RuleFor(x => x.CreateDate)
             .LessThanOrEqualTo(DateTime.Now).WithMessage("CreateDate cannot be in the future");

        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Firstname is required")
            .MaximumLength(50).WithMessage("Firstname must not exceed 50 characters");

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Lastname is required")
            .MaximumLength(50).WithMessage("Lastname must not exceed 50 characters");

        RuleFor(x => x.JobPosition)
            .NotEmpty().WithMessage("JobPosition is required")
            .MaximumLength(50).WithMessage("JobPosition must not exceed 50 characters");

        RuleFor(x => x.Salary)
            .InclusiveBetween(0, 10000).WithMessage("Salary must be between 0 and 10000");

RuleFor(x => x.WorkExperience)
            .GreaterThanOrEqualTo(0).WithMessage("WorkExperience must be greater than or equal to 0");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Address is required")
            .SetValidator(new PersonAddressCreateDtoValidator());
    }
}