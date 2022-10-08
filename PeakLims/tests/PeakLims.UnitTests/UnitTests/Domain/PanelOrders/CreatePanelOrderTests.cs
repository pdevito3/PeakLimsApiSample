namespace PeakLims.UnitTests.UnitTests.Domain.PanelOrders;

using PeakLims.SharedTestHelpers.Fakes.PanelOrder;
using PeakLims.Domain.PanelOrders;
using PeakLims.Domain.PanelOrders.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreatePanelOrderTests
{
    private readonly Faker _faker;

    public CreatePanelOrderTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_panelOrder()
    {
        // Arrange + Act
        var panelOrderToCreate = new FakePanelOrderForCreationDto().Generate();
        var fakePanelOrder = FakePanelOrder.Generate(panelOrderToCreate);

        // Assert
        fakePanelOrder.Status.Should().Be(panelOrderToCreate.Status);
        fakePanelOrder.PanelId.Should().Be(panelOrderToCreate.PanelId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakePanelOrder = FakePanelOrder.Generate();

        // Assert
        fakePanelOrder.DomainEvents.Count.Should().Be(1);
        fakePanelOrder.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PanelOrderCreated));
    }
}