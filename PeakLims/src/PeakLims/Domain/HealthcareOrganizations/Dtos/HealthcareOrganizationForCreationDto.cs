namespace PeakLims.Domain.HealthcareOrganizations.Dtos;

using Addresses.Dtos;

public sealed class HealthcareOrganizationForCreationDto : HealthcareOrganizationForManipulationDto
{
    public AddressForCreationDto PrimaryAddress { get; set; }
}
