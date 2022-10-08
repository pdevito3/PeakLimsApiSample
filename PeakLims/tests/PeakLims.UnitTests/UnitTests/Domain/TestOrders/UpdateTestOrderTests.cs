namespace PeakLims.UnitTests.UnitTests.Domain.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateTestOrderTests
{
    private readonly Faker _faker;

    public UpdateTestOrderTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_testOrder()
    {
        // Arrange
        var fakeTestOrder = FakeTestOrder.Generate();
        var updatedTestOrder = new FakeTestOrderForUpdateDto().Generate();
        
        // Act
        fakeTestOrder.Update(updatedTestOrder);

        // Assert
        fakeTestOrder.State.Should().Be(updatedTestOrder.State);
        fakeTestOrder.TestId.Should().Be(updatedTestOrder.TestId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeTestOrder = FakeTestOrder.Generate();
        var updatedTestOrder = new FakeTestOrderForUpdateDto().Generate();
        fakeTestOrder.DomainEvents.Clear();
        
        // Act
        fakeTestOrder.Update(updatedTestOrder);

        // Assert
        fakeTestOrder.DomainEvents.Count.Should().Be(1);
        fakeTestOrder.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestOrderUpdated));
    }
}