namespace PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;

using AutoBogus;
using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;

public class FakeHealthcareOrganizationContact
{
    public static HealthcareOrganizationContact Generate(HealthcareOrganizationContactForCreationDto healthcareOrganizationContactForCreationDto)
    {
        return HealthcareOrganizationContact.Create(healthcareOrganizationContactForCreationDto);
    }

    public static HealthcareOrganizationContact Generate()
    {
        return HealthcareOrganizationContact.Create(new FakeHealthcareOrganizationContactForCreationDto().Generate());
    }
}