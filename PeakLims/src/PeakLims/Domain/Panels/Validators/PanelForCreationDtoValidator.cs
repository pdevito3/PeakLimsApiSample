namespace PeakLims.Domain.Panels.Validators;

using PeakLims.Domain.Panels.Dtos;
using FluentValidation;

public sealed class PanelForCreationDtoValidator: PanelForManipulationDtoValidator<PanelForCreationDto>
{
    public PanelForCreationDtoValidator()
    {
        RuleFor(x => x.PanelCode).NotEmpty();
    }
}