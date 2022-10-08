namespace PeakLims.IntegrationTests.FeatureTests.PanelOrders;

using PeakLims.SharedTestHelpers.Fakes.PanelOrder;
using PeakLims.Domain.PanelOrders.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.PanelOrders.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Panel;

public class UpdatePanelOrderCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_panelorder_in_db()
    {
        // Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        await InsertAsync(fakePanelOne);

        var fakePanelOrderOne = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id).Generate());
        var updatedPanelOrderDto = new FakePanelOrderForUpdateDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id).Generate();
        await InsertAsync(fakePanelOrderOne);

        var panelOrder = await ExecuteDbContextAsync(db => db.PanelOrders
            .FirstOrDefaultAsync(p => p.Id == fakePanelOrderOne.Id));
        var id = panelOrder.Id;

        // Act
        var command = new UpdatePanelOrder.Command(id, updatedPanelOrderDto);
        await SendAsync(command);
        var updatedPanelOrder = await ExecuteDbContextAsync(db => db.PanelOrders.FirstOrDefaultAsync(p => p.Id == id));

        // Assert
        updatedPanelOrder.State.Should().Be(updatedPanelOrderDto.State);
        updatedPanelOrder.PanelId.Should().Be(updatedPanelOrderDto.PanelId);
    }
}