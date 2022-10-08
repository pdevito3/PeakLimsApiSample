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

public class DeletePanelOrderTests : TestBase
{
    [Test]
    public async Task delete_panelorder_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
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
        var route = ApiRoutes.PanelOrders.Delete.Replace(ApiRoutes.PanelOrders.Id, fakePanelOrder.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_panelorder_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakePanelOrder = FakePanelOrder.Generate(new FakePanelOrderForCreationDto().Generate());

        await InsertAsync(fakePanelOrder);

        // Act
        var route = ApiRoutes.PanelOrders.Delete.Replace(ApiRoutes.PanelOrders.Id, fakePanelOrder.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_panelorder_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakePanelOrder = FakePanelOrder.Generate(new FakePanelOrderForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakePanelOrder);

        // Act
        var route = ApiRoutes.PanelOrders.Delete.Replace(ApiRoutes.PanelOrders.Id, fakePanelOrder.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}