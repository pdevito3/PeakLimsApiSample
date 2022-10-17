namespace PeakLims.Domain.Samples.Validators;

using PeakLims.Domain.Samples.Dtos;
using FluentValidation;

public sealed class SampleForCreationDtoValidator: SampleForManipulationDtoValidator<ContainerlessSampleForCreationDto>
{
    public SampleForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}