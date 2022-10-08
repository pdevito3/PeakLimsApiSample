namespace PeakLims.Domain.PanelOrders.Validators;

using PeakLims.Domain.PanelOrders.Dtos;
using FluentValidation;

public sealed class PanelOrderForUpdateDtoValidator: PanelOrderForManipulationDtoValidator<PanelOrderForUpdateDto>
{
    public PanelOrderForUpdateDtoValidator()
    {
        // add fluent validation rules that should only be run on update operations here
        //https://fluentvalidation.net/
    }
}