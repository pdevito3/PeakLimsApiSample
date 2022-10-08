namespace PeakLims.Domain.Ethnicities.Mappings;

using Mapster;

public sealed class EthnicityMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, Ethnicity>()
            .MapWith(value => new Ethnicity(value));
        config.NewConfig<Ethnicity, string>()
            .MapWith(role => role.Value);
    }
}