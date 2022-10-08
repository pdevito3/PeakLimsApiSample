namespace PeakLims.Domain.TestOrders.Validators;

using PeakLims.Domain.TestOrders.Dtos;
using FluentValidation;

public sealed class TestOrderForUpdateDtoValidator: TestOrderForManipulationDtoValidator<TestOrderForUpdateDto>
{
    public TestOrderForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}