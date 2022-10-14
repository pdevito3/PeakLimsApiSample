namespace PeakLims.UnitTests.UnitTests.Domain.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using PeakLims.Domain.TestOrderStatuses;

[Parallelizable]
public class CreateTestOrderTests
{
    private readonly Faker _faker;

    public CreateTestOrderTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_testOrder()
    {
        // Arrange + Act
        var testOrderToCreate = new FakeTestOrderForCreationDto().Generate();
        var fakeTestOrder = FakeTestOrder.Generate(testOrderToCreate);

        // Assert
        fakeTestOrder.Status.Should().Be(TestOrderStatus.Pending());
        fakeTestOrder.TestId.Should().Be(testOrderToCreate.TestId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeTestOrder = FakeTestOrder.Generate();

        // Assert
        fakeTestOrder.DomainEvents.Count.Should().Be(1);
        fakeTestOrder.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestOrderCreated));
    }
}