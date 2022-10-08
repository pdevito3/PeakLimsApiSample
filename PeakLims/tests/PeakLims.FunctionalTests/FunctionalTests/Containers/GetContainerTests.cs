namespace PeakLims.FunctionalTests.FunctionalTests.Containers;

using PeakLims.SharedTestHelpers.Fakes.Container;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetContainerTests : TestBase
{
    [Test]
    public async Task get_container_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeContainer);

        // Act
        var route = ApiRoutes.Containers.GetRecord.Replace(ApiRoutes.Containers.Id, fakeContainer.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_container_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());

        await InsertAsync(fakeContainer);

        // Act
        var route = ApiRoutes.Containers.GetRecord.Replace(ApiRoutes.Containers.Id, fakeContainer.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_container_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeContainer = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakeContainer);

        // Act
        var route = ApiRoutes.Containers.GetRecord.Replace(ApiRoutes.Containers.Id, fakeContainer.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}