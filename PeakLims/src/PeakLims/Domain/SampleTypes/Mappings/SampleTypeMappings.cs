namespace PeakLims.Domain.SampleTypes.Mappings;

using Mapster;

public sealed class RaceMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, SampleType>()
            .MapWith(value => new SampleType(value));
        config.NewConfig<SampleType, string>()
            .MapWith(sampleType => sampleType.Value);
    }
}