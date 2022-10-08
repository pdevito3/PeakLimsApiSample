namespace PeakLims.Domain.HealthcareOrganizationContacts.Validators;

using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using FluentValidation;

public sealed class HealthcareOrganizationContactForCreationDtoValidator: HealthcareOrganizationContactForManipulationDtoValidator<HealthcareOrganizationContactForCreationDto>
{
    public HealthcareOrganizationContactForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}