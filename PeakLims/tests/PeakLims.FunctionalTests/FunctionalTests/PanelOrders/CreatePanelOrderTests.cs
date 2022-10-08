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

public class CreatePanelOrderTests : TestBase
{
    [Test]
    public async Task create_panelorder_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        await InsertAsync(fakePanelOne);

        var fakePanelOrder = new FakePanelOrderForCreationDto()
            .RuleFor(p => p.PanelId, _ => fakePanelOne.Id)
            .Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.PanelOrders.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakePanelOrder);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_panelorder_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakePanelOrder = new FakePanelOrderForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.PanelOrders.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakePanelOrder);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_panelorder_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakePanelOrder = new FakePanelOrderForCreationDto { }.Generate();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.PanelOrders.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakePanelOrder);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}