namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizationContacts;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using PeakLims.Domain.HealthcareOrganizationContacts.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

public class HealthcareOrganizationContactQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_healthcareorganizationcontact_with_accurate_props()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());
        await InsertAsync(fakeHealthcareOrganizationContactOne);

        // Act
        var query = new GetHealthcareOrganizationContact.Query(fakeHealthcareOrganizationContactOne.Id);
        var healthcareOrganizationContact = await SendAsync(query);

        // Assert
        healthcareOrganizationContact.Name.Should().Be(fakeHealthcareOrganizationContactOne.Name);
        healthcareOrganizationContact.Email.Should().Be(fakeHealthcareOrganizationContactOne.Email);
        healthcareOrganizationContact.Npi.Should().Be(fakeHealthcareOrganizationContactOne.Npi);
        healthcareOrganizationContact.HealthcareOrganizationId.Should().Be(fakeHealthcareOrganizationContactOne.HealthcareOrganizationId);
    }

    [Test]
    public async Task get_healthcareorganizationcontact_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetHealthcareOrganizationContact.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}