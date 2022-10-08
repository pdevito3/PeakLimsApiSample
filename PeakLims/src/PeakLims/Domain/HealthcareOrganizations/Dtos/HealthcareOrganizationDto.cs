namespace PeakLims.Domain.HealthcareOrganizations.Dtos;

using Addresses.Dtos;

public sealed class HealthcareOrganizationDto 
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public AddressDto PrimaryAddress { get; set; }
}
