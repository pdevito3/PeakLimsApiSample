namespace PeakLims.Domain.PanelOrders.Mappings;

using PeakLims.Domain.PanelOrders.Dtos;
using PeakLims.Domain.PanelOrders;
using Mapster;

public sealed class PanelOrderMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PanelOrder, PanelOrderDto>();
        config.NewConfig<PanelOrderForCreationDto, PanelOrder>()
            .TwoWays();
        config.NewConfig<PanelOrderForUpdateDto, PanelOrder>()
            .TwoWays();
    }
}