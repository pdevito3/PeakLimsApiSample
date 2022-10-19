namespace PeakLims.FunctionalTests.FunctionalTests.Panels;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class UpdatePanelRecordTests : TestBase
{
    [Test]
    public async Task put_panel_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var panel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();
        var updatedPanelDto = new FakePanelForUpdateDto().Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(panel);

        // Act
        var route = ApiRoutes.Panels.Put.Replace(ApiRoutes.Panels.Id, panel.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedPanelDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task put_panel_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var panel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();
        var updatedPanelDto = new FakePanelForUpdateDto { }.Generate();

        await InsertAsync(panel);

        // Act
        var route = ApiRoutes.Panels.Put.Replace(ApiRoutes.Panels.Id, panel.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedPanelDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task put_panel_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var panel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();
        var updatedPanelDto = new FakePanelForUpdateDto { }.Generate();
        FactoryClient.AddAuth();

        await InsertAsync(panel);

        // Act
        var route = ApiRoutes.Panels.Put.Replace(ApiRoutes.Panels.Id, panel.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedPanelDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}