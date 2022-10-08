namespace PeakLims.Domain.AccessionComments.Validators;

using PeakLims.Domain.AccessionComments.Dtos;
using FluentValidation;

public sealed class AccessionCommentForCreationDtoValidator: AccessionCommentForManipulationDtoValidator<AccessionCommentForCreationDto>
{
    public AccessionCommentForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}