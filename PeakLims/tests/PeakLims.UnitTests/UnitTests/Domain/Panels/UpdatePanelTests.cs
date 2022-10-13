namespace PeakLims.UnitTests.UnitTests.Domain.Panels;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using NUnit.Framework;
using PeakLims.Domain.Panels.Services;

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
        var fakePanel = new FakePanelBuilder()
            .WithMockRepository(false)
            .Build();
        var updatedPanel = new FakePanelForUpdateDto().Generate();
        
        var mockPanelRepository = new Mock<IPanelRepository>();
        mockPanelRepository
            .Setup(x => x.Exists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        // Act
        fakePanel.Update(updatedPanel, mockPanelRepository.Object);

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
        var fakePanel = new FakePanelBuilder()
            .WithMockRepository(false)
            .Build();
        var updatedPanel = new FakePanelForUpdateDto().Generate();
        fakePanel.DomainEvents.Clear();
        
        var mockPanelRepository = new Mock<IPanelRepository>();
        mockPanelRepository
            .Setup(x => x.Exists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        // Act
        fakePanel.Update(updatedPanel, mockPanelRepository.Object);

        // Assert
        fakePanel.DomainEvents.Count.Should().Be(1);
        fakePanel.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PanelUpdated));
    }
}