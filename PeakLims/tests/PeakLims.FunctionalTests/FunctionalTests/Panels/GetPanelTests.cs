namespace PeakLims.FunctionalTests.FunctionalTests.Panels;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetPanelTests : TestBase
{
    [Test]
    public async Task get_panel_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakePanel = FakePanel.Generate(new FakePanelForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakePanel);

        // Act
        var route = ApiRoutes.Panels.GetRecord.Replace(ApiRoutes.Panels.Id, fakePanel.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_panel_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakePanel = FakePanel.Generate(new FakePanelForCreationDto().Generate());

        await InsertAsync(fakePanel);

        // Act
        var route = ApiRoutes.Panels.GetRecord.Replace(ApiRoutes.Panels.Id, fakePanel.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_panel_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakePanel = FakePanel.Generate(new FakePanelForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakePanel);

        // Act
        var route = ApiRoutes.Panels.GetRecord.Replace(ApiRoutes.Panels.Id, fakePanel.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}