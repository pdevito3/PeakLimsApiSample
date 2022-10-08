namespace PeakLims.Domain.HealthcareOrganizationContacts.Dtos;

public abstract class HealthcareOrganizationContactForManipulationDto 
{
        public string Name { get; set; }
        public string Email { get; set; }
        public string Npi { get; set; }
        public Guid HealthcareOrganizationId { get; set; }
}
