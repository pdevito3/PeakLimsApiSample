namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizationContacts;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.HealthcareOrganizationContacts.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

public class AddHealthcareOrganizationContactCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_healthcareorganizationcontact_to_db()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeHealthcareOrganizationContactOne = new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate();

        // Act
        var command = new AddHealthcareOrganizationContact.Command(fakeHealthcareOrganizationContactOne);
        var healthcareOrganizationContactReturned = await SendAsync(command);
        var healthcareOrganizationContactCreated = await ExecuteDbContextAsync(db => db.HealthcareOrganizationContacts
            .FirstOrDefaultAsync(h => h.Id == healthcareOrganizationContactReturned.Id));

        // Assert
        healthcareOrganizationContactReturned.Name.Should().Be(fakeHealthcareOrganizationContactOne.Name);
        healthcareOrganizationContactReturned.Email.Should().Be(fakeHealthcareOrganizationContactOne.Email);
        healthcareOrganizationContactReturned.Npi.Should().Be(fakeHealthcareOrganizationContactOne.Npi);
        healthcareOrganizationContactReturned.HealthcareOrganizationId.Should().Be(fakeHealthcareOrganizationContactOne.HealthcareOrganizationId);

        healthcareOrganizationContactCreated.Name.Should().Be(fakeHealthcareOrganizationContactOne.Name);
        healthcareOrganizationContactCreated.Email.Should().Be(fakeHealthcareOrganizationContactOne.Email);
        healthcareOrganizationContactCreated.Npi.Should().Be(fakeHealthcareOrganizationContactOne.Npi);
        healthcareOrganizationContactCreated.HealthcareOrganizationId.Should().Be(fakeHealthcareOrganizationContactOne.HealthcareOrganizationId);
    }
}