namespace PeakLims.Domain.Containers.Validators;

using PeakLims.Domain.Containers.Dtos;
using FluentValidation;

public sealed class ContainerForCreationDtoValidator: ContainerForManipulationDtoValidator<ContainerForCreationDto>
{
    public ContainerForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}