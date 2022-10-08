namespace PeakLims.Domain.Tests.Mappings;

using PeakLims.Domain.Tests.Dtos;
using PeakLims.Domain.Tests;
using Mapster;

public sealed class TestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Test, TestDto>();
        config.NewConfig<TestForCreationDto, Test>()
            .TwoWays();
        config.NewConfig<TestForUpdateDto, Test>()
            .TwoWays();
    }
}