namespace PeakLims.IntegrationTests.FeatureTests.Panels;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.Panels.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddPanelCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_panel_to_db()
    {
        // Arrange
        var fakePanelOne = new FakePanelForCreationDto().Generate();

        // Act
        var command = new AddPanel.Command(fakePanelOne);
        var panelReturned = await SendAsync(command);
        var panelCreated = await ExecuteDbContextAsync(db => db.Panels
            .FirstOrDefaultAsync(p => p.Id == panelReturned.Id));

        // Assert
        panelReturned.PanelCode.Should().Be(fakePanelOne.PanelCode);
        panelReturned.PanelName.Should().Be(fakePanelOne.PanelName);
        panelReturned.Type.Should().Be(fakePanelOne.Type);
        panelReturned.Version.Should().Be(fakePanelOne.Version);
        
        panelCreated.PanelCode.Should().Be(fakePanelOne.PanelCode);
        panelCreated.PanelName.Should().Be(fakePanelOne.PanelName);
        panelCreated.Type.Should().Be(fakePanelOne.Type);
        panelCreated.Version.Should().Be(fakePanelOne.Version);
    }
    
    [Test]
    public async Task can_not_add_panel_with_same_code_and_version()
    {
        // Arrange
        var fakePanelOne = new FakePanelForCreationDto().Generate();
        var fakePanelTwo = new FakePanelForCreationDto().Generate();
        fakePanelTwo.PanelCode = fakePanelOne.PanelCode;
        fakePanelTwo.Version = fakePanelOne.Version;

        // Act
        var commandOne = new AddPanel.Command(fakePanelOne);
        await SendAsync(commandOne);
        var commandTwo = new AddPanel.Command(fakePanelTwo);
        var act = () => SendAsync(commandTwo);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}