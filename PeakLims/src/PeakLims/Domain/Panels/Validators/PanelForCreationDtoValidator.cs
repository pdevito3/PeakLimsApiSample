namespace PeakLims.Domain.Panels.Validators;

using PeakLims.Domain.Panels.Dtos;
using FluentValidation;

public sealed class PanelForCreationDtoValidator: PanelForManipulationDtoValidator<PanelForCreationDto>
{
    public PanelForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}