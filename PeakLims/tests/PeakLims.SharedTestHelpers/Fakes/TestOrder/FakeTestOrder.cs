namespace PeakLims.SharedTestHelpers.Fakes.TestOrder;

using AutoBogus;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.Dtos;

public class FakeTestOrder
{
    public static TestOrder Generate(Guid testId)
    {
        return TestOrder.Create(testId);
    }

    public static TestOrder Generate()
    {
        return TestOrder.Create(Guid.NewGuid());
    }
}