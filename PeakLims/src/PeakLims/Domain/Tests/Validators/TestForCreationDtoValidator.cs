namespace PeakLims.Domain.Tests.Validators;

using PeakLims.Domain.Tests.Dtos;
using FluentValidation;

public sealed class TestForCreationDtoValidator: TestForManipulationDtoValidator<TestForCreationDto>
{
    public TestForCreationDtoValidator()
    {
        RuleFor(x => x.TestCode).NotEmpty();
    }
}