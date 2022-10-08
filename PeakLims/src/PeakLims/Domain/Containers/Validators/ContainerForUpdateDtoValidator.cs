namespace PeakLims.Domain.Containers.Validators;

using PeakLims.Domain.Containers.Dtos;
using FluentValidation;

public sealed class ContainerForUpdateDtoValidator: ContainerForManipulationDtoValidator<ContainerForUpdateDto>
{
    public ContainerForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}