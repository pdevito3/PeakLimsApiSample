namespace PeakLims.Domain.Accessions.Validators;

using PeakLims.Domain.Accessions.Dtos;
using FluentValidation;

public sealed class AccessionForCreationDtoValidator: AccessionForManipulationDtoValidator<AccessionForCreationDto>
{
    public AccessionForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}