namespace PeakLims.IntegrationTests.FeatureTests.PanelOrders;

using PeakLims.SharedTestHelpers.Fakes.PanelOrder;
using PeakLims.Domain.PanelOrders.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Domain.Panels.Services;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Panel;

public class DeletePanelOrderCommandTests : TestBase
{
    [Test]
    public async Task can_delete_panelorder_from_db()
    {
        // Arrange
        var fakePanelOne = new FakePanelBuilder()
            .WithRepository(GetService<IPanelRepository>())
            .Build();
        await InsertAsync(fakePanelOne);

        var fakePanelOrderOne = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id).Generate());
        await InsertAsync(fakePanelOrderOne);
        var panelOrder = await ExecuteDbContextAsync(db => db.PanelOrders
            .FirstOrDefaultAsync(p => p.Id == fakePanelOrderOne.Id));

        // Act
        var command = new DeletePanelOrder.Command(panelOrder.Id);
        await SendAsync(command);
        var panelOrderResponse = await ExecuteDbContextAsync(db => db.PanelOrders.CountAsync(p => p.Id == panelOrder.Id));

        // Assert
        panelOrderResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_panelorder_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeletePanelOrder.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}