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
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
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
        fakePanel.Type.Should().Be(updatedPanel.Type);
        fakePanel.Version.Should().Be(updatedPanel.Version);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();
        var updatedPanel = new FakePanelForUpdateDto().Generate();
        fakePanel.DomainEvents.Clear();
        
        var mockPanelRepository = new Mock<IPanelRepository>();
        
        // Act
        fakePanel.Update(updatedPanel, mockPanelRepository.Object);

        // Assert
        fakePanel.DomainEvents.Count.Should().Be(1);
        fakePanel.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PanelUpdated));
    }
    
    [Test]
    public void panel_must_have_name()
    {
        // Arrange + Act
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();
        var updatedPanel = new FakePanelForUpdateDto().Generate();
        updatedPanel.PanelName = null;
        
        // Act
        var act = () => fakePanel.Update(updatedPanel, Mock.Of<IPanelRepository>());

        // Assert
        act.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void panel_must_have_version_greater_than_or_equal_to_zero()
    {
        // Arrange
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();
        var updatedPanel = new FakePanelForUpdateDto().Generate();
        updatedPanel.Version = -1;
        
        // Act
        var act = () => fakePanel.Update(updatedPanel, Mock.Of<IPanelRepository>());

        // Assert
        act.Should().Throw<FluentValidation.ValidationException>();
    }
}