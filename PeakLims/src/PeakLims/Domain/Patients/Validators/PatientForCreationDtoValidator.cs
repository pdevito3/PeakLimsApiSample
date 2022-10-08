namespace PeakLims.Domain.Patients.Validators;

using PeakLims.Domain.Patients.Dtos;
using FluentValidation;

public sealed class PatientForCreationDtoValidator: PatientForManipulationDtoValidator<PatientForCreationDto>
{
    public PatientForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}