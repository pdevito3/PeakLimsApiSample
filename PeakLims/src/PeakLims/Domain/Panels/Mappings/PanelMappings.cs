namespace PeakLims.Domain.Panels.Mappings;

using PeakLims.Domain.Panels.Dtos;
using PeakLims.Domain.Panels;
using Mapster;

public sealed class PanelMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Panel, PanelDto>();
        config.NewConfig<PanelForCreationDto, Panel>()
            .TwoWays();
        config.NewConfig<PanelForUpdateDto, Panel>()
            .TwoWays();
    }
}