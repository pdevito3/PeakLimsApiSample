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

public class UpdatePanelOrderRecordTests : TestBase
{
    [Test]
    public async Task put_panelorder_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        await InsertAsync(fakePanelOne);

        var fakePanelOrder = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id).Generate());
        var updatedPanelOrderDto = new FakePanelOrderForUpdateDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id).Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakePanelOrder);

        // Act
        var route = ApiRoutes.PanelOrders.Put.Replace(ApiRoutes.PanelOrders.Id, fakePanelOrder.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedPanelOrderDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task put_panelorder_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakePanelOrder = FakePanelOrder.Generate(new FakePanelOrderForCreationDto().Generate());
        var updatedPanelOrderDto = new FakePanelOrderForUpdateDto { }.Generate();

        await InsertAsync(fakePanelOrder);

        // Act
        var route = ApiRoutes.PanelOrders.Put.Replace(ApiRoutes.PanelOrders.Id, fakePanelOrder.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedPanelOrderDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task put_panelorder_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakePanelOrder = FakePanelOrder.Generate(new FakePanelOrderForCreationDto().Generate());
        var updatedPanelOrderDto = new FakePanelOrderForUpdateDto { }.Generate();
        FactoryClient.AddAuth();

        await InsertAsync(fakePanelOrder);

        // Act
        var route = ApiRoutes.PanelOrders.Put.Replace(ApiRoutes.PanelOrders.Id, fakePanelOrder.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedPanelOrderDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}