namespace PeakLims.Domain.Accessions.Dtos;

public sealed class AccessionDto 
{
        public Guid Id { get; set; }
        public string AccessionNumber { get; set; }
        public string State { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? HealthcareOrganizationId { get; set; }

}
