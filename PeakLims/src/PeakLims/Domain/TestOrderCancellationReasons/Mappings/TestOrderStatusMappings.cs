namespace PeakLims.Domain.TestOrderCancellationReasons.Mappings;

using Mapster;
using TestOrderCancellationReasons;

public sealed class TestOrderCancellationReasonMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, TestOrderCancellationReason>()
            .MapWith(value => new TestOrderCancellationReason(value));
        config.NewConfig<TestOrderCancellationReason, string>()
            .MapWith(role => role.Value);
    }
}