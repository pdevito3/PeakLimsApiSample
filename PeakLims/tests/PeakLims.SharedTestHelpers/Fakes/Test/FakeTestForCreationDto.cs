namespace PeakLims.SharedTestHelpers.Fakes.Test;

using AutoBogus;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeTestForCreationDto : AutoFaker<TestForCreationDto>
{
    public FakeTestForCreationDto()
    {
        // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
        //RuleFor(t => t.ExampleIntProperty, t => t.Random.Number(50, 100000));
        //RuleFor(t => t.ExampleDateProperty, t => t.Date.Past());
    }
}