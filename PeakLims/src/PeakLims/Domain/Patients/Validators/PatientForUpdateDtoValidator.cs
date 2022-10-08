namespace PeakLims.Domain.Patients.Validators;

using PeakLims.Domain.Patients.Dtos;
using FluentValidation;

public sealed class PatientForUpdateDtoValidator: PatientForManipulationDtoValidator<PatientForUpdateDto>
{
    public PatientForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}