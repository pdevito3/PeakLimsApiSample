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

public class GetHealthcareOrganizationContactTests : TestBase
{
    [Test]
    public async Task get_healthcareorganizationcontact_returns_success_when_entity_exists_using_valid_auth_credentials()
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
        var route = ApiRoutes.HealthcareOrganizationContacts.GetRecord.Replace(ApiRoutes.HealthcareOrganizationContacts.Id, fakeHealthcareOrganizationContact.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_healthcareorganizationcontact_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeHealthcareOrganizationContact = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto().Generate());

        await InsertAsync(fakeHealthcareOrganizationContact);

        // Act
        var route = ApiRoutes.HealthcareOrganizationContacts.GetRecord.Replace(ApiRoutes.HealthcareOrganizationContacts.Id, fakeHealthcareOrganizationContact.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_healthcareorganizationcontact_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeHealthcareOrganizationContact = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakeHealthcareOrganizationContact);

        // Act
        var route = ApiRoutes.HealthcareOrganizationContacts.GetRecord.Replace(ApiRoutes.HealthcareOrganizationContacts.Id, fakeHealthcareOrganizationContact.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}