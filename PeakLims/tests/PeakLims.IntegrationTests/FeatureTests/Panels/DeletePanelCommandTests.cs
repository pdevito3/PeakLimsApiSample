namespace PeakLims.IntegrationTests.FeatureTests.Panels;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using PeakLims.Domain.Panels.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Domain.Panels.Services;
using static TestFixture;

public class DeletePanelCommandTests : TestBase
{
    [Test]
    public async Task can_delete_panel_from_db()
    {
        // Arrange
        var fakePanelOne = new FakePanelBuilder()
            .WithRepository(GetService<IPanelRepository>())
            .Build();
        await InsertAsync(fakePanelOne);
        var panel = await ExecuteDbContextAsync(db => db.Panels
            .FirstOrDefaultAsync(p => p.Id == fakePanelOne.Id));

        // Act
        var command = new DeletePanel.Command(panel.Id);
        await SendAsync(command);
        var panelResponse = await ExecuteDbContextAsync(db => db.Panels.CountAsync(p => p.Id == panel.Id));

        // Assert
        panelResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_panel_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeletePanel.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_panel_from_db()
    {
        // Arrange
        var fakePanelOne = new FakePanelBuilder()
            .WithRepository(GetService<IPanelRepository>())
            .Build();
        await InsertAsync(fakePanelOne);
        var panel = await ExecuteDbContextAsync(db => db.Panels
            .FirstOrDefaultAsync(p => p.Id == fakePanelOne.Id));

        // Act
        var command = new DeletePanel.Command(panel.Id);
        await SendAsync(command);
        var deletedPanel = await ExecuteDbContextAsync(db => db.Panels
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == panel.Id));

        // Assert
        deletedPanel?.IsDeleted.Should().BeTrue();
    }
}