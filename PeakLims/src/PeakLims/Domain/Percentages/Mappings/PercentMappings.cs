namespace PeakLims.Domain.Percentages.Mappings;

using SharedKernel.Domain;
using Mapster;

public sealed class PercentMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<decimal, Percent>()
            .MapWith(value => new Percent(value));
        config.NewConfig<Percent, decimal>()
            .MapWith(percent => percent.Value);
    }
}