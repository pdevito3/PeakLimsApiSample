namespace PeakLims.Domain.ContainerStatuses.Mappings;

using Mapster;

public sealed class PanelStatusMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, ContainerStatus>()
            .MapWith(value => new ContainerStatus(value));
        config.NewConfig<ContainerStatus, string>()
            .MapWith(role => role.Value);
    }
}