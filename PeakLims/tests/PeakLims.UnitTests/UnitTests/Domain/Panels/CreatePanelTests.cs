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
using PeakLims.Domain.PanelStatuses;

[Parallelizable]
public class CreatePanelTests
{
    private readonly Faker _faker;

    public CreatePanelTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_panel()
    {
        // Arrange + Act
        var panelToCreate = new FakePanelForCreationDto().Generate();
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .WithDto(panelToCreate)
            .Build();

        // Assert
        fakePanel.PanelCode.Should().Be(panelToCreate.PanelCode);
        fakePanel.PanelName.Should().Be(panelToCreate.PanelName);
        fakePanel.Type.Should().Be(panelToCreate.Type);
        fakePanel.Version.Should().Be(panelToCreate.Version);
        fakePanel.Status.Should().Be(PanelStatus.Draft());
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();

        // Assert
        fakePanel.DomainEvents.Count.Should().Be(1);
        fakePanel.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PanelCreated));
    }
    
    [Test]
    public void panel_must_have_name()
    {
        // Arrange
        var panelToCreate = new FakePanelForCreationDto().Generate();
        panelToCreate.PanelName = null;
        
        // Act
        var fakePanel = () => Panel.Create(panelToCreate, Mock.Of<IPanelRepository>());

        // Assert
        fakePanel.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void panel_must_have_code()
    {
        // Arrange
        var panelToCreate = new FakePanelForCreationDto().Generate();
        panelToCreate.PanelCode = null;
        
        // Act
        var fakePanel = () => Panel.Create(panelToCreate, Mock.Of<IPanelRepository>());

        // Assert
        fakePanel.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void panel_must_have_version_greater_than_or_equal_to_zero()
    {
        // Arrange
        var panelToCreate = new FakePanelForCreationDto().Generate();
        panelToCreate.Version = -1;
        
        // Act
        var fakePanel = () => Panel.Create(panelToCreate, Mock.Of<IPanelRepository>());

        // Assert
        fakePanel.Should().Throw<FluentValidation.ValidationException>();
    }
}