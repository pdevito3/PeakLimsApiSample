namespace PeakLims.SharedTestHelpers.Fakes.Panel;

using AutoBogus;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakePanelForCreationDto : AutoFaker<PanelForCreationDto>
{
    public FakePanelForCreationDto()
    {
        // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
        //RuleFor(p => p.ExampleIntProperty, p => p.Random.Number(50, 100000));
        //RuleFor(p => p.ExampleDateProperty, p => p.Date.Past());
    }
}