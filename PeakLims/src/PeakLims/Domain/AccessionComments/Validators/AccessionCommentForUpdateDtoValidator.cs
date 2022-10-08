namespace PeakLims.Domain.AccessionComments.Validators;

using PeakLims.Domain.AccessionComments.Dtos;
using FluentValidation;

public sealed class AccessionCommentForUpdateDtoValidator: AccessionCommentForManipulationDtoValidator<AccessionCommentForUpdateDto>
{
    public AccessionCommentForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}