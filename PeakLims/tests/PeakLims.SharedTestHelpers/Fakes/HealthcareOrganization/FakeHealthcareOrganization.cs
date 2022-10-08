namespace PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

using AutoBogus;
using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizations.Dtos;

public class FakeHealthcareOrganization
{
    public static HealthcareOrganization Generate(HealthcareOrganizationForCreationDto healthcareOrganizationForCreationDto)
    {
        return HealthcareOrganization.Create(healthcareOrganizationForCreationDto);
    }

    public static HealthcareOrganization Generate()
    {
        return HealthcareOrganization.Create(new FakeHealthcareOrganizationForCreationDto().Generate());
    }
}