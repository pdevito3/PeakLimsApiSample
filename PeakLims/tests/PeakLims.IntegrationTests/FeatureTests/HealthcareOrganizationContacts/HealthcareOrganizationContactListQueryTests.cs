namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizationContacts;

using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using SharedKernel.Exceptions;
using PeakLims.Domain.HealthcareOrganizationContacts.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

public class HealthcareOrganizationContactListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_healthcareorganizationcontact_list()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        var fakeHealthcareOrganizationTwo = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne, fakeHealthcareOrganizationTwo);

        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());
        var fakeHealthcareOrganizationContactTwo = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.HealthcareOrganizationId, _ => fakeHealthcareOrganizationTwo.Id).Generate());
        var queryParameters = new HealthcareOrganizationContactParametersDto();

        await InsertAsync(fakeHealthcareOrganizationContactOne, fakeHealthcareOrganizationContactTwo);

        // Act
        var query = new GetHealthcareOrganizationContactList.Query(queryParameters);
        var healthcareOrganizationContacts = await SendAsync(query);

        // Assert
        healthcareOrganizationContacts.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}