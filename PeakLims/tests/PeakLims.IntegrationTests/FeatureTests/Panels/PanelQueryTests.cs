namespace PeakLims.IntegrationTests.FeatureTests.Panels;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using PeakLims.Domain.Panels.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Domain.Panels.Services;
using static TestFixture;

public class PanelQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_panel_with_accurate_props()
    {
        // Arrange
        var fakePanelOne = new FakePanelBuilder()
            .WithRepository(GetService<IPanelRepository>())
            .Build();
        await InsertAsync(fakePanelOne);

        // Act
        var query = new GetPanel.Query(fakePanelOne.Id);
        var panel = await SendAsync(query);

        // Assert
        panel.PanelCode.Should().Be(fakePanelOne.PanelCode);
        panel.PanelName.Should().Be(fakePanelOne.PanelName);
        panel.Type.Should().Be(fakePanelOne.Type);
        panel.Version.Should().Be(fakePanelOne.Version);
    }

    [Test]
    public async Task get_panel_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetPanel.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}