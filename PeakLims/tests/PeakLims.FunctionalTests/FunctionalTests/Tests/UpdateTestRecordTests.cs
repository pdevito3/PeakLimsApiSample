namespace PeakLims.FunctionalTests.FunctionalTests.Tests;

using PeakLims.SharedTestHelpers.Fakes.Test;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Domain.Tests.Services;
using Moq;

public class UpdateTestRecordTests : TestBase
{
    [Test]
    public async Task put_test_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeTest = FakeTest.Generate(new FakeTestForCreationDto().Generate(), Mock.Of<ITestRepository>());
        var updatedTestDto = new FakeTestForUpdateDto().Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeTest);

        // Act
        var route = ApiRoutes.Tests.Put.Replace(ApiRoutes.Tests.Id, fakeTest.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedTestDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task put_test_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeTest = FakeTest.Generate(new FakeTestForCreationDto().Generate(), Mock.Of<ITestRepository>());
        var updatedTestDto = new FakeTestForUpdateDto { }.Generate();

        await InsertAsync(fakeTest);

        // Act
        var route = ApiRoutes.Tests.Put.Replace(ApiRoutes.Tests.Id, fakeTest.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedTestDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task put_test_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeTest = FakeTest.Generate(new FakeTestForCreationDto().Generate(), Mock.Of<ITestRepository>());
        var updatedTestDto = new FakeTestForUpdateDto { }.Generate();
        FactoryClient.AddAuth();

        await InsertAsync(fakeTest);

        // Act
        var route = ApiRoutes.Tests.Put.Replace(ApiRoutes.Tests.Id, fakeTest.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedTestDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}