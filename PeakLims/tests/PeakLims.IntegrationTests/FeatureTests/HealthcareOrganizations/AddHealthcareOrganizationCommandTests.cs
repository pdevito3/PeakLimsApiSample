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

        healthcareOrganizationCreated.Name.Should().Be(fakeHealthcareOrganizationOne.Name);
        healthcareOrganizationCreated.Email.Should().Be(fakeHealthcareOrganizationOne.Email);
    }
}