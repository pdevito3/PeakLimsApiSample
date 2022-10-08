namespace PeakLims.Domain.Accessions.Dtos;

public abstract class AccessionForManipulationDto 
{
        public string Status { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? HealthcareOrganizationId { get; set; }

}
