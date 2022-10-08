namespace PeakLims.Domain.Sexes.Mappings;

using Mapster;

public sealed class SexMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, Sex>()
            .MapWith(value => new Sex(value));
        config.NewConfig<Sex, string>()
            .MapWith(role => role.Value);
    }
}