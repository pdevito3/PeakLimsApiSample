namespace PeakLims.FunctionalTests.FunctionalTests.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateHealthcareOrganizationTests : TestBase
{
    [Test]
    public async Task create_healthcareorganization_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeHealthcareOrganization = new FakeHealthcareOrganizationForCreationDto().Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.HealthcareOrganizations.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeHealthcareOrganization);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_healthcareorganization_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeHealthcareOrganization = new FakeHealthcareOrganizationForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.HealthcareOrganizations.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeHealthcareOrganization);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_healthcareorganization_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeHealthcareOrganization = new FakeHealthcareOrganizationForCreationDto { }.Generate();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.HealthcareOrganizations.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeHealthcareOrganization);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}