namespace PeakLims.Domain.Panels.Validators;

using PeakLims.Domain.Panels.Dtos;
using FluentValidation;

public class PanelForManipulationDtoValidator<T> : AbstractValidator<T> where T : PanelForManipulationDto
{
    public PanelForManipulationDtoValidator()
    {
        RuleFor(x => x.PanelName).NotEmpty();
        RuleFor(x => x.Version)
            .GreaterThanOrEqualTo(0)
            .NotEmpty();
    }
}