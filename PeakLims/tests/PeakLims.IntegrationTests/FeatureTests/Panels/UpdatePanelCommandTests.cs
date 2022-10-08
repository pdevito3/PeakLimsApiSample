namespace PeakLims.IntegrationTests.FeatureTests.Panels;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using PeakLims.Domain.Panels.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.Panels.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class UpdatePanelCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_panel_in_db()
    {
        // Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        var updatedPanelDto = new FakePanelForUpdateDto().Generate();
        await InsertAsync(fakePanelOne);

        var panel = await ExecuteDbContextAsync(db => db.Panels
            .FirstOrDefaultAsync(p => p.Id == fakePanelOne.Id));
        var id = panel.Id;

        // Act
        var command = new UpdatePanel.Command(id, updatedPanelDto);
        await SendAsync(command);
        var updatedPanel = await ExecuteDbContextAsync(db => db.Panels.FirstOrDefaultAsync(p => p.Id == id));

        // Assert
        updatedPanel.PanelNumber.Should().Be(updatedPanelDto.PanelNumber);
        updatedPanel.PanelCode.Should().Be(updatedPanelDto.PanelCode);
        updatedPanel.PanelName.Should().Be(updatedPanelDto.PanelName);
        updatedPanel.TurnAroundTime.Should().Be(updatedPanelDto.TurnAroundTime);
        updatedPanel.Type.Should().Be(updatedPanelDto.Type);
        updatedPanel.Version.Should().Be(updatedPanelDto.Version);
    }
}