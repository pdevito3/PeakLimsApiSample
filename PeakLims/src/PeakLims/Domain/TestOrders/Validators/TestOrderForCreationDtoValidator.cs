namespace PeakLims.Domain.TestOrders.Validators;

using PeakLims.Domain.TestOrders.Dtos;
using FluentValidation;

public sealed class TestOrderForCreationDtoValidator: TestOrderForManipulationDtoValidator<TestOrderForCreationDto>
{
    public TestOrderForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}