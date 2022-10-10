namespace PeakLims.Domain.AccessionStatuses.Mappings;

using Mapster;

public sealed class AccessionStatusMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, AccessionStatus>()
            .MapWith(value => new AccessionStatus(value));
        config.NewConfig<AccessionStatus, string>()
            .MapWith(role => role.Value);
    }
}