namespace PeakLims.Domain.Users.Validators;

using PeakLims.Domain.Users.Dtos;
using PeakLims.Domain;
using FluentValidation;

public class UserForManipulationDtoValidator<T> : AbstractValidator<T> where T : UserForManipulationDto
{
    public UserForManipulationDtoValidator()
    {
        RuleFor(u => u.Identifier)
            .NotEmpty()
            .WithMessage("Please provide an identifier.");
    }
}