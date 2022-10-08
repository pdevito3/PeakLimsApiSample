namespace PeakLims.UnitTests.UnitTests.Domain.PanelOrders;

using PeakLims.SharedTestHelpers.Fakes.PanelOrder;
using PeakLims.Domain.PanelOrders;
using PeakLims.Domain.PanelOrders.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdatePanelOrderTests
{
    private readonly Faker _faker;

    public UpdatePanelOrderTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_panelOrder()
    {
        // Arrange
        var fakePanelOrder = FakePanelOrder.Generate();
        var updatedPanelOrder = new FakePanelOrderForUpdateDto().Generate();
        
        // Act
        fakePanelOrder.Update(updatedPanelOrder);

        // Assert
        fakePanelOrder.State.Should().Be(updatedPanelOrder.State);
        fakePanelOrder.PanelId.Should().Be(updatedPanelOrder.PanelId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakePanelOrder = FakePanelOrder.Generate();
        var updatedPanelOrder = new FakePanelOrderForUpdateDto().Generate();
        fakePanelOrder.DomainEvents.Clear();
        
        // Act
        fakePanelOrder.Update(updatedPanelOrder);

        // Assert
        fakePanelOrder.DomainEvents.Count.Should().Be(1);
        fakePanelOrder.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PanelOrderUpdated));
    }
}