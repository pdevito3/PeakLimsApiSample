namespace PeakLims.Domain.Panels.Validators;

using PeakLims.Domain.Panels.Dtos;
using FluentValidation;

public sealed class PanelForUpdateDtoValidator: PanelForManipulationDtoValidator<PanelForUpdateDto>
{
    public PanelForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}