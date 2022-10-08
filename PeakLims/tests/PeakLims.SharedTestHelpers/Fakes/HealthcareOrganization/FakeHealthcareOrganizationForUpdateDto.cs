namespace PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

using Address;
using AutoBogus;
using PeakLims.Domain.HealthcareOrganizations.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeHealthcareOrganizationForUpdateDto : AutoFaker<HealthcareOrganizationForUpdateDto>
{
    public FakeHealthcareOrganizationForUpdateDto()
    {
        RuleFor(u => u.Email, f => f.Person.Email);
        RuleFor(u => u.PrimaryAddress, _ => new FakeAddressForUpdateDto().Generate());
    }
}