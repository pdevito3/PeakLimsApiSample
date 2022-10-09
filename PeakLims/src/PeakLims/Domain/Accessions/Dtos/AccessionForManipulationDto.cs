namespace PeakLims.Domain.Accessions.Dtos;

public abstract class AccessionForManipulationDto 
{
        public Guid? PatientId { get; set; }
        public Guid? HealthcareOrganizationId { get; set; }

}
