namespace PeakLims.Domain.Tests.Validators;

using PeakLims.Domain.Tests.Dtos;
using FluentValidation;

public sealed class TestForCreationDtoValidator: TestForManipulationDtoValidator<TestForCreationDto>
{
    public TestForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}