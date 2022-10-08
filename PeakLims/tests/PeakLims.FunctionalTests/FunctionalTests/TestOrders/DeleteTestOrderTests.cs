namespace PeakLims.FunctionalTests.FunctionalTests.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using PeakLims.SharedTestHelpers.Fakes.Test;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteTestOrderTests : TestBase
{
    [Test]
    public async Task delete_testorder_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        await InsertAsync(fakeTestOne);

        var fakeTestOrder = FakeTestOrder.Generate(new FakeTestOrderForCreationDto()
            .RuleFor(t => t.TestId, _ => fakeTestOne.Id).Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeTestOrder);

        // Act
        var route = ApiRoutes.TestOrders.Delete.Replace(ApiRoutes.TestOrders.Id, fakeTestOrder.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_testorder_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeTestOrder = FakeTestOrder.Generate(new FakeTestOrderForCreationDto().Generate());

        await InsertAsync(fakeTestOrder);

        // Act
        var route = ApiRoutes.TestOrders.Delete.Replace(ApiRoutes.TestOrders.Id, fakeTestOrder.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_testorder_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeTestOrder = FakeTestOrder.Generate(new FakeTestOrderForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakeTestOrder);

        // Act
        var route = ApiRoutes.TestOrders.Delete.Replace(ApiRoutes.TestOrders.Id, fakeTestOrder.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}