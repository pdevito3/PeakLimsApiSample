namespace PeakLims.Domain.TestOrders.Mappings;

using PeakLims.Domain.TestOrders.Dtos;
using PeakLims.Domain.TestOrders;
using Mapster;

public sealed class TestOrderMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TestOrder, TestOrderDto>();
        config.NewConfig<TestOrderForCreationDto, TestOrder>()
            .TwoWays();
        config.NewConfig<TestOrderForUpdateDto, TestOrder>()
            .TwoWays();
    }
}