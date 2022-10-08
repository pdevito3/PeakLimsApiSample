namespace PeakLims.Domain.MonetaryAmounts.Mappings;

using SharedKernel.Domain;
using Mapster;

public sealed class MonetaryAmountMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<decimal, MonetaryAmount>()
            .MapWith(value => new MonetaryAmount(value));
        config.NewConfig<MonetaryAmount, decimal>()
            .MapWith(monetaryAmount => monetaryAmount.Amount);
    }
}