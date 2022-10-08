namespace PeakLims.Domain.HealthcareOrganizationContacts.Validators;

using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using FluentValidation;

public sealed class HealthcareOrganizationContactForUpdateDtoValidator: HealthcareOrganizationContactForManipulationDtoValidator<HealthcareOrganizationContactForUpdateDto>
{
    public HealthcareOrganizationContactForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}