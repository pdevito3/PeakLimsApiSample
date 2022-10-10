namespace PeakLims.UnitTests.UnitTests.Domain.Panels;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdatePanelTests
{
    private readonly Faker _faker;

    public UpdatePanelTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_panel()
    {
        // Arrange
        var fakePanel = FakePanel.Generate();
        var updatedPanel = new FakePanelForUpdateDto().Generate();
        
        // Act
        fakePanel.Update(updatedPanel);

        // Assert
        fakePanel.PanelName.Should().Be(updatedPanel.PanelName);
        fakePanel.TurnAroundTime.Should().Be(updatedPanel.TurnAroundTime);
        fakePanel.Type.Should().Be(updatedPanel.Type);
        fakePanel.Version.Should().Be(updatedPanel.Version);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakePanel = FakePanel.Generate();
        var updatedPanel = new FakePanelForUpdateDto().Generate();
        fakePanel.DomainEvents.Clear();
        
        // Act
        fakePanel.Update(updatedPanel);

        // Assert
        fakePanel.DomainEvents.Count.Should().Be(1);
        fakePanel.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PanelUpdated));
    }
}