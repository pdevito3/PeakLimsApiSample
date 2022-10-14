namespace PeakLims.Domain.PanelStatuses.Mappings;

using PanelStatuses;
using Mapster;

public sealed class PanelStatusMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, PanelStatus>()
            .MapWith(value => new PanelStatus(value));
        config.NewConfig<PanelStatus, string>()
            .MapWith(role => role.Value);
    }
}