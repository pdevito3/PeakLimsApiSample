namespace PeakLims.FunctionalTests.FunctionalTests.PanelOrders;

using PeakLims.SharedTestHelpers.Fakes.PanelOrder;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using PeakLims.SharedTestHelpers.Fakes.Panel;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetPanelOrderTests : TestBase
{
    [Test]
    public async Task get_panelorder_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        await InsertAsync(fakePanelOne);

        var fakePanelOrder = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id).Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakePanelOrder);

        // Act
        var route = ApiRoutes.PanelOrders.GetRecord.Replace(ApiRoutes.PanelOrders.Id, fakePanelOrder.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_panelorder_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakePanelOrder = FakePanelOrder.Generate(new FakePanelOrderForCreationDto().Generate());

        await InsertAsync(fakePanelOrder);

        // Act
        var route = ApiRoutes.PanelOrders.GetRecord.Replace(ApiRoutes.PanelOrders.Id, fakePanelOrder.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_panelorder_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakePanelOrder = FakePanelOrder.Generate(new FakePanelOrderForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakePanelOrder);

        // Act
        var route = ApiRoutes.PanelOrders.GetRecord.Replace(ApiRoutes.PanelOrders.Id, fakePanelOrder.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}