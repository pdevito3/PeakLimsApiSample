namespace PeakLims.Domain.Samples.Validators;

using PeakLims.Domain.Samples.Dtos;
using FluentValidation;

public sealed class SampleForUpdateDtoValidator: SampleForManipulationDtoValidator<SampleForUpdateDto>
{
    public SampleForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}