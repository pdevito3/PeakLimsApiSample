namespace PeakLims.Domain.HealthcareOrganizations.Validators;

using PeakLims.Domain.HealthcareOrganizations.Dtos;
using FluentValidation;

public sealed class HealthcareOrganizationForCreationDtoValidator: HealthcareOrganizationForManipulationDtoValidator<HealthcareOrganizationForCreationDto>
{
    public HealthcareOrganizationForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}