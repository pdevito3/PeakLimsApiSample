namespace PeakLims.SharedTestHelpers.Fakes.TestOrder;

using AutoBogus;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeTestOrderForUpdateDto : AutoFaker<TestOrderForUpdateDto>
{
    public FakeTestOrderForUpdateDto()
    {
        // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
        //RuleFor(t => t.ExampleIntProperty, t => t.Random.Number(50, 100000));
        //RuleFor(t => t.ExampleDateProperty, t => t.Date.Past());
    }
}