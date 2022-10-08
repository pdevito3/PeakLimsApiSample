namespace PeakLims.Domain.PanelOrders.Validators;

using PeakLims.Domain.PanelOrders.Dtos;
using FluentValidation;

public sealed class PanelOrderForCreationDtoValidator: PanelOrderForManipulationDtoValidator<PanelOrderForCreationDto>
{
    public PanelOrderForCreationDtoValidator()
    {
        // add fluent validation rules that should only be run on creation operations here
        //https://fluentvalidation.net/
    }
}