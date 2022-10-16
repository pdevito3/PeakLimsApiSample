namespace PeakLims.Domain.Races.Mappings;

using Mapster;

public sealed class RaceMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, Race>()
            .MapWith(value => new Race(value));
        config.NewConfig<Race, string>()
            .MapWith(race => race.Value);
    }
}