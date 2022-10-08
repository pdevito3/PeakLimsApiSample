namespace PeakLims.Domain.HealthcareOrganizations.Validators;

using PeakLims.Domain.HealthcareOrganizations.Dtos;
using FluentValidation;

public sealed class HealthcareOrganizationForUpdateDtoValidator: HealthcareOrganizationForManipulationDtoValidator<HealthcareOrganizationForUpdateDto>
{
    public HealthcareOrganizationForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}