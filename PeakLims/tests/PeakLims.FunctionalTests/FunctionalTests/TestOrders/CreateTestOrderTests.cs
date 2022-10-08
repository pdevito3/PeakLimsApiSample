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

public class CreateTestOrderTests : TestBase
{
    [Test]
    public async Task create_testorder_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        await InsertAsync(fakeTestOne);

        var fakeTestOrder = new FakeTestOrderForCreationDto()
            .RuleFor(t => t.TestId, _ => fakeTestOne.Id)
            .Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.TestOrders.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeTestOrder);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_testorder_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeTestOrder = new FakeTestOrderForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.TestOrders.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeTestOrder);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_testorder_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeTestOrder = new FakeTestOrderForCreationDto { }.Generate();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.TestOrders.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeTestOrder);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}