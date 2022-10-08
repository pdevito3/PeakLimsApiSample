namespace PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;

using AutoBogus;
using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeHealthcareOrganizationContactForUpdateDto : AutoFaker<HealthcareOrganizationContactForUpdateDto>
{
    public FakeHealthcareOrganizationContactForUpdateDto()
    {
        RuleFor(u => u.Email, f => f.Person.Email);
    }
}