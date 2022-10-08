namespace PeakLims.SharedTestHelpers.Fakes.PanelOrder;

using AutoBogus;
using PeakLims.Domain.PanelOrders;
using PeakLims.Domain.PanelOrders.Dtos;

public class FakePanelOrder
{
    public static PanelOrder Generate(PanelOrderForCreationDto panelOrderForCreationDto)
    {
        return PanelOrder.Create(panelOrderForCreationDto);
    }

    public static PanelOrder Generate()
    {
        return PanelOrder.Create(new FakePanelOrderForCreationDto().Generate());
    }
}