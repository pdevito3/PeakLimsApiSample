namespace PeakLims.Domain.HealthcareOrganizations.Dtos;

using Addresses.Dtos;

public sealed class HealthcareOrganizationForUpdateDto : HealthcareOrganizationForManipulationDto
{
    public AddressForUpdateDto PrimaryAddress { get; set; }
}
