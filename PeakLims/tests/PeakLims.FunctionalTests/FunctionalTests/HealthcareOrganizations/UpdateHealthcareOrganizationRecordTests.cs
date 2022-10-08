namespace PeakLims.FunctionalTests.FunctionalTests.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class UpdateHealthcareOrganizationRecordTests : TestBase
{
    [Test]
    public async Task put_healthcareorganization_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        var updatedHealthcareOrganizationDto = new FakeHealthcareOrganizationForUpdateDto().Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeHealthcareOrganization);

        // Act
        var route = ApiRoutes.HealthcareOrganizations.Put.Replace(ApiRoutes.HealthcareOrganizations.Id, fakeHealthcareOrganization.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedHealthcareOrganizationDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task put_healthcareorganization_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        var updatedHealthcareOrganizationDto = new FakeHealthcareOrganizationForUpdateDto { }.Generate();

        await InsertAsync(fakeHealthcareOrganization);

        // Act
        var route = ApiRoutes.HealthcareOrganizations.Put.Replace(ApiRoutes.HealthcareOrganizations.Id, fakeHealthcareOrganization.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedHealthcareOrganizationDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task put_healthcareorganization_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        var updatedHealthcareOrganizationDto = new FakeHealthcareOrganizationForUpdateDto { }.Generate();
        FactoryClient.AddAuth();

        await InsertAsync(fakeHealthcareOrganization);

        // Act
        var route = ApiRoutes.HealthcareOrganizations.Put.Replace(ApiRoutes.HealthcareOrganizations.Id, fakeHealthcareOrganization.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedHealthcareOrganizationDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}