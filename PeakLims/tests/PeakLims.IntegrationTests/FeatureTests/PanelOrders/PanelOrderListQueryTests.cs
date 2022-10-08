namespace PeakLims.IntegrationTests.FeatureTests.PanelOrders;

using PeakLims.Domain.PanelOrders.Dtos;
using PeakLims.SharedTestHelpers.Fakes.PanelOrder;
using SharedKernel.Exceptions;
using PeakLims.Domain.PanelOrders.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Panel;

public class PanelOrderListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_panelorder_list()
    {
        // Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        await InsertAsync(fakePanelOne, fakePanelTwo);

        var fakePanelOrderOne = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id).Generate());
        var fakePanelOrderTwo = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.PanelId, _ => fakePanelTwo.Id).Generate());
        var queryParameters = new PanelOrderParametersDto();

        await InsertAsync(fakePanelOrderOne, fakePanelOrderTwo);

        // Act
        var query = new GetPanelOrderList.Query(queryParameters);
        var panelOrders = await SendAsync(query);

        // Assert
        panelOrders.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}