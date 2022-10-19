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

public class GetTestTests : TestBase
{
    [Test]
    public async Task get_test_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakeTest = FakeTest.Generate(new FakeTestForCreationDto().Generate(), Mock.Of<ITestRepository>());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeTest);

        // Act
        var route = ApiRoutes.Tests.GetRecord.Replace(ApiRoutes.Tests.Id, fakeTest.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_test_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeTest = FakeTest.Generate(new FakeTestForCreationDto().Generate(), Mock.Of<ITestRepository>());

        await InsertAsync(fakeTest);

        // Act
        var route = ApiRoutes.Tests.GetRecord.Replace(ApiRoutes.Tests.Id, fakeTest.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_test_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeTest = FakeTest.Generate(new FakeTestForCreationDto().Generate(), Mock.Of<ITestRepository>());
        FactoryClient.AddAuth();

        await InsertAsync(fakeTest);

        // Act
        var route = ApiRoutes.Tests.GetRecord.Replace(ApiRoutes.Tests.Id, fakeTest.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}