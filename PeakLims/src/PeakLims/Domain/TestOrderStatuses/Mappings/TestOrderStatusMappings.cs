namespace PeakLims.Domain.TestOrderStatuses.Mappings;

using Mapster;

public sealed class TestOrderStatusMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, TestOrderStatus>()
            .MapWith(value => new TestOrderStatus(value));
        config.NewConfig<TestOrderStatus, string>()
            .MapWith(role => role.Value);
    }
}