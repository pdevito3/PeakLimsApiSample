namespace PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

using AutoBogus;
using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizations.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeHealthcareOrganizationForUpdateDto : AutoFaker<HealthcareOrganizationForUpdateDto>
{
    public FakeHealthcareOrganizationForUpdateDto()
    {
        // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
        //RuleFor(h => h.ExampleIntProperty, h => h.Random.Number(50, 100000));
        //RuleFor(h => h.ExampleDateProperty, h => h.Date.Past());
    }
}