namespace PeakLims.SharedTestHelpers.Fakes.TestOrder;

using AutoBogus;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.Dtos;

public class FakeTestOrder
{
    public static TestOrder Generate(TestOrderForCreationDto testOrderForCreationDto)
    {
        return TestOrder.Create(testOrderForCreationDto);
    }

    public static TestOrder Generate()
    {
        return TestOrder.Create(new FakeTestOrderForCreationDto().Generate());
    }
}