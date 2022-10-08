namespace PeakLims.Domain.Patients.Dtos;

using Lifespans.Dtos;

public sealed class PatientForCreationDto : PatientForManipulationDto
{
    public LifespanForCreationDto Lifespan { get; set; } = new LifespanForCreationDto();
}
