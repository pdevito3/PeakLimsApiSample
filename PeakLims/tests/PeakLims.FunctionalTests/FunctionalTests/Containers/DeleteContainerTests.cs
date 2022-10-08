namespace PeakLims.FunctionalTests.FunctionalTests.Containers;

using PeakLims.SharedTestHelpers.Fakes.Container;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteContainerTests : TestBase
{
    [Test]
    public async Task delete_container_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeContainer);

        // Act
        var route = ApiRoutes.Containers.Delete.Replace(ApiRoutes.Containers.Id, fakeContainer.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_container_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());

        await InsertAsync(fakeContainer);

        // Act
        var route = ApiRoutes.Containers.Delete.Replace(ApiRoutes.Containers.Id, fakeContainer.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_container_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakeContainer);

        // Act
        var route = ApiRoutes.Containers.Delete.Replace(ApiRoutes.Containers.Id, fakeContainer.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}