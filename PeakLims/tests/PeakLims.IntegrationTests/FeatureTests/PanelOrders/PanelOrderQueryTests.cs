namespace PeakLims.IntegrationTests.FeatureTests.PanelOrders;

using PeakLims.SharedTestHelpers.Fakes.PanelOrder;
using PeakLims.Domain.PanelOrders.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Domain.Panels.Services;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Panel;

public class PanelOrderQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_panelorder_with_accurate_props()
    {
        // Arrange
        var fakePanelOne = new FakePanelBuilder()
            .WithRepository(GetService<IPanelRepository>())
            .Build();
        await InsertAsync(fakePanelOne);

        var fakePanelOrderOne = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id).Generate());
        await InsertAsync(fakePanelOrderOne);

        // Act
        var query = new GetPanelOrder.Query(fakePanelOrderOne.Id);
        var panelOrder = await SendAsync(query);

        // Assert
        panelOrder.Status.Should().Be(fakePanelOrderOne.Status);
        panelOrder.PanelId.Should().Be(fakePanelOrderOne.PanelId);
    }

    [Test]
    public async Task get_panelorder_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetPanelOrder.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}