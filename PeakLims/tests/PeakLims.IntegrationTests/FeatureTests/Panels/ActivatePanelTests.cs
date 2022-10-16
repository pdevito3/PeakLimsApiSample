namespace PeakLims.IntegrationTests.FeatureTests.Panels;

using System.Threading.Tasks;
using Domain.TestOrders.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PeakLims.Domain.Panels.Features;
using PeakLims.Domain.Panels.Services;
using PeakLims.Domain.PanelStatuses;
using PeakLims.SharedTestHelpers.Fakes.Panel;
using static TestFixture;

public class ActivatePanelTests : TestBase
{
    [Test]
    public async Task can_activate_panel()
    {
        // Arrange
        var fakePanel = FakePanelBuilder
            .Initialize()
            .WithPanelRepository(GetService<IPanelRepository>())
            .WithTestOrderRepository(GetService<ITestOrderRepository>())
            .Build();
        await InsertAsync(fakePanel);

        // Act
        var command = new ActivatePanel.Command(fakePanel.Id);
        await SendAsync(command);
        var updatedPanel = await ExecuteDbContextAsync(db => db.Panels.FirstOrDefaultAsync(a => a.Id == fakePanel.Id));

        // Assert
        updatedPanel.Status.Should().Be(PanelStatus.Active());
    }
}