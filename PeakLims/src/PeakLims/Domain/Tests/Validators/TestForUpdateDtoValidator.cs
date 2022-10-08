namespace PeakLims.Domain.Tests.Validators;

using PeakLims.Domain.Tests.Dtos;
using FluentValidation;

public sealed class TestForUpdateDtoValidator: TestForManipulationDtoValidator<TestForUpdateDto>
{
    public TestForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}