namespace PeakLims.Domain.Tests.Validators;

using PeakLims.Domain.Tests.Dtos;
using FluentValidation;

public class TestForManipulationDtoValidator<T> : AbstractValidator<T> where T : TestForManipulationDto
{
    public TestForManipulationDtoValidator()
    {
        RuleFor(x => x.TurnAroundTime)
            .GreaterThanOrEqualTo(0)
            .NotEmpty();
        RuleFor(x => x.TestName).NotEmpty();
        RuleFor(x => x.Version)
            .GreaterThanOrEqualTo(0)
            .NotEmpty();
    }
}