namespace PeakLims.IntegrationTests.FeatureTests.PanelOrders;

using PeakLims.SharedTestHelpers.Fakes.PanelOrder;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.PanelOrders.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using PeakLims.SharedTestHelpers.Fakes.Panel;

public class AddPanelOrderCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_panelorder_to_db()
    {
        // Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        await InsertAsync(fakePanelOne);

        var fakePanelOrderOne = new FakePanelOrderForCreationDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id).Generate();

        // Act
        var command = new AddPanelOrder.Command(fakePanelOrderOne);
        var panelOrderReturned = await SendAsync(command);
        var panelOrderCreated = await ExecuteDbContextAsync(db => db.PanelOrders
            .FirstOrDefaultAsync(p => p.Id == panelOrderReturned.Id));

        // Assert
        panelOrderReturned.State.Should().Be(fakePanelOrderOne.State);
        panelOrderReturned.PanelId.Should().Be(fakePanelOrderOne.PanelId);

        panelOrderCreated.State.Should().Be(fakePanelOrderOne.State);
        panelOrderCreated.PanelId.Should().Be(fakePanelOrderOne.PanelId);
    }
}