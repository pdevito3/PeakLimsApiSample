namespace PeakLims.Domain.Accessions.Dtos;

public class AccessionForCreationDto
{
    public Guid? PatientId { get; set; }
    public Guid? HealthcareOrganizationId { get; set; }
}