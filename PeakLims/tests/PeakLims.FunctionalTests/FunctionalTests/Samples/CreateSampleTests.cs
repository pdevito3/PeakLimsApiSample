namespace PeakLims.FunctionalTests.FunctionalTests.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.SharedTestHelpers.Fakes.Container;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Services;

public class CreateSampleTests : TestBase
{
    [Test]
    public async Task create_sample_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var container = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(container);
        var fakeSample = new FakeSampleForCreationDto(container).Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.Samples.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeSample);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_sample_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeSample = new FakeContainerlessSampleForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.Samples.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeSample);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_sample_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeSample = new FakeContainerlessSampleForCreationDto { }.Generate();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.Samples.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeSample);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}