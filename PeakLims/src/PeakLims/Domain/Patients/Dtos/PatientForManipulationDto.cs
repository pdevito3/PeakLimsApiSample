namespace PeakLims.Domain.Patients.Dtos;

public abstract class PatientForManipulationDto 
{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public string InternalId { get; set; }

}
