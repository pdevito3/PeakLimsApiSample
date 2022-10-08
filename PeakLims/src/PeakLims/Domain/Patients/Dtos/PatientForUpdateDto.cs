namespace PeakLims.Domain.Patients.Dtos;

using Lifespans.Dtos;

public sealed class PatientForUpdateDto : PatientForManipulationDto
{
    public LifespanForUpdateDto Lifespan { get; set; } = new LifespanForUpdateDto();
}
