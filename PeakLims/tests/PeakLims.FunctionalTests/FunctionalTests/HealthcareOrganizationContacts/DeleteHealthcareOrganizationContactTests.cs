namespace PeakLims.FunctionalTests.FunctionalTests.HealthcareOrganizationContacts;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class DeleteHealthcareOrganizationContactTests : TestBase
{
    [Test]
    public async Task delete_healthcareorganizationcontact_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeHealthcareOrganizationContact = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeHealthcareOrganizationContact);

        // Act
        var route = ApiRoutes.HealthcareOrganizationContacts.Delete.Replace(ApiRoutes.HealthcareOrganizationContacts.Id, fakeHealthcareOrganizationContact.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_healthcareorganizationcontact_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeHealthcareOrganizationContact = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto().Generate());

        // Act
        var route = ApiRoutes.HealthcareOrganizationContacts.Delete.Replace(ApiRoutes.HealthcareOrganizationContacts.Id, fakeHealthcareOrganizationContact.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_healthcareorganizationcontact_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeHealthcareOrganizationContact = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto().Generate());
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.HealthcareOrganizationContacts.Delete.Replace(ApiRoutes.HealthcareOrganizationContacts.Id, fakeHealthcareOrganizationContact.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}