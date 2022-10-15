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
using Domain.Panels.Services;
using static TestFixture;

public class UpdatePanelCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_panel_in_db()
    {
        // Arrange
        var fakePanelOne = new FakePanelBuilder()
            .WithRepository(GetService<IPanelRepository>())
            .Build();
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
        updatedPanel.PanelName.Should().Be(updatedPanelDto.PanelName);
        updatedPanel.Type.Should().Be(updatedPanelDto.Type);
        updatedPanel.Version.Should().Be(updatedPanelDto.Version);
    }
    
    [Test]
    public async Task can_not_update_panel_with_same_code_and_version()
    {
        // Arrange
        var fakePanelOne = new FakePanelBuilder()
            .WithRepository(GetService<IPanelRepository>())
            .Build();
        await InsertAsync(fakePanelOne);
        var fakePanelTwo = new FakePanelForUpdateDto().Generate();
        fakePanelTwo.Version = fakePanelOne.Version;

        // Act
        var command = new UpdatePanel.Command(fakePanelOne.Id, fakePanelTwo);
        var act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}