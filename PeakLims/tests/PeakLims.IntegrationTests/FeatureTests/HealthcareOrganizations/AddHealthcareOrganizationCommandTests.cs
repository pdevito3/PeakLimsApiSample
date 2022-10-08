namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.HealthcareOrganizations.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddHealthcareOrganizationCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_healthcareorganization_to_db()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = new FakeHealthcareOrganizationForCreationDto().Generate();

        // Act
        var command = new AddHealthcareOrganization.Command(fakeHealthcareOrganizationOne);
        var healthcareOrganizationReturned = await SendAsync(command);
        var healthcareOrganizationCreated = await ExecuteDbContextAsync(db => db.HealthcareOrganizations
            .FirstOrDefaultAsync(h => h.Id == healthcareOrganizationReturned.Id));

        // Assert
        healthcareOrganizationReturned.Name.Should().Be(fakeHealthcareOrganizationOne.Name);
        healthcareOrganizationReturned.Email.Should().Be(fakeHealthcareOrganizationOne.Email);
        healthcareOrganizationReturned.PrimaryAddress.Line1.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.Line1);
        healthcareOrganizationReturned.PrimaryAddress.Line2.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.Line2);
        healthcareOrganizationReturned.PrimaryAddress.City.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.City);
        healthcareOrganizationReturned.PrimaryAddress.State.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.State);
        healthcareOrganizationReturned.PrimaryAddress.PostalCode.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.PostalCode);
        healthcareOrganizationReturned.PrimaryAddress.Country.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.Country);

        healthcareOrganizationCreated?.Name.Should().Be(fakeHealthcareOrganizationOne.Name);
        healthcareOrganizationCreated?.Email.Value.Should().Be(fakeHealthcareOrganizationOne.Email);
        healthcareOrganizationCreated?.PrimaryAddress.Line1.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.Line1);
        healthcareOrganizationCreated?.PrimaryAddress.Line2.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.Line2);
        healthcareOrganizationCreated?.PrimaryAddress.City.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.City);
        healthcareOrganizationCreated?.PrimaryAddress.State.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.State);
        healthcareOrganizationCreated?.PrimaryAddress.PostalCode.Value.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.PostalCode);
        healthcareOrganizationCreated?.PrimaryAddress.Country.Should().Be(fakeHealthcareOrganizationOne.PrimaryAddress.Country);
    }
}