namespace PeakLims.Domain.Accessions.Validators;

using PeakLims.Domain.Accessions.Dtos;
using FluentValidation;

public sealed class AccessionForUpdateDtoValidator: AccessionForManipulationDtoValidator<AccessionForUpdateDto>
{
    public AccessionForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}