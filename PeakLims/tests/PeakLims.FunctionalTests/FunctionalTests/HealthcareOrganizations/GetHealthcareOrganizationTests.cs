namespace PeakLims.FunctionalTests.FunctionalTests.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetHealthcareOrganizationTests : TestBase
{
    [Test]
    public async Task get_healthcareorganization_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeHealthcareOrganization);

        // Act
        var route = ApiRoutes.HealthcareOrganizations.GetRecord.Replace(ApiRoutes.HealthcareOrganizations.Id, fakeHealthcareOrganization.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_healthcareorganization_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());

        await InsertAsync(fakeHealthcareOrganization);

        // Act
        var route = ApiRoutes.HealthcareOrganizations.GetRecord.Replace(ApiRoutes.HealthcareOrganizations.Id, fakeHealthcareOrganization.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_healthcareorganization_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakeHealthcareOrganization);

        // Act
        var route = ApiRoutes.HealthcareOrganizations.GetRecord.Replace(ApiRoutes.HealthcareOrganizations.Id, fakeHealthcareOrganization.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}