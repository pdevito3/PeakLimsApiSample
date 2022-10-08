namespace PeakLims.Domain.Patients.Dtos;

using Lifespans.Dtos;

public sealed class PatientDto 
{
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public LifespanDto Lifespan { get; set; } = new LifespanDto();
        public string Sex { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public string InternalId { get; set; }
}
