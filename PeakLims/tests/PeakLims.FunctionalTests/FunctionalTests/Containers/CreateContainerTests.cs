namespace PeakLims.FunctionalTests.FunctionalTests.Containers;

using PeakLims.SharedTestHelpers.Fakes.Container;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateContainerTests : TestBase
{
    [Test]
    public async Task create_container_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeContainer = new FakeContainerForCreationDto().Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.Containers.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeContainer);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_container_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeContainer = new FakeContainerForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.Containers.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeContainer);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_container_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeContainer = new FakeContainerForCreationDto { }.Generate();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.Containers.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeContainer);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}