namespace PeakLims.UnitTests.UnitTests.Domain.Panels;

using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.Panels.DomainEvents;
using PeakLims.Domain.PanelStatuses;
using SharedTestHelpers.Fakes.Panel;

[Parallelizable]
public class PanelStateChangeTests
{
    private readonly Faker _faker;

    public PanelStateChangeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_activate_panel()
    {
        // Arrange
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();
        fakePanel.DomainEvents.Clear();
        
        // Act
        fakePanel.Activate();

        // Assert
        fakePanel.Status.Should().Be(PanelStatus.Active());
        fakePanel.DomainEvents.Count.Should().Be(1);
        fakePanel.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PanelUpdated));
    }
    
    [Test]
    public void can_deactivate_panel()
    {
        // Arrange
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();
        fakePanel.DomainEvents.Clear();
        
        // Act
        fakePanel.Deactivate();

        // Assert
        fakePanel.Status.Should().Be(PanelStatus.Inactive());
        fakePanel.DomainEvents.Count.Should().Be(1);
        fakePanel.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PanelUpdated));
    }
}