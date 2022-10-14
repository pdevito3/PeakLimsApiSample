namespace PeakLims.Domain.TestStatuses.Mappings;

using TestStatuses;
using Mapster;

public sealed class TestStatusMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, TestStatus>()
            .MapWith(value => new TestStatus(value));
        config.NewConfig<TestStatus, string>()
            .MapWith(role => role.Value);
    }
}