namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizations;

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PeakLims.Domain.HealthcareOrganizations.Features;
using PeakLims.Domain.HealthcareOrganizationStatuses;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using static TestFixture;

public class ActivateHealthcareOrganizationTests : TestBase
{
    [Test]
    public async Task can_activate_healthcareOrganization()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate();
        fakeHealthcareOrganization.Deactivate();
        await InsertAsync(fakeHealthcareOrganization);

        // Act
        var command = new ActivateHealthcareOrganization.Command(fakeHealthcareOrganization.Id);
        await SendAsync(command);
        var updatedHealthcareOrganization = await ExecuteDbContextAsync(db => db.HealthcareOrganizations.FirstOrDefaultAsync(a => a.Id == fakeHealthcareOrganization.Id));

        // Assert
        updatedHealthcareOrganization.Status.Should().Be(HealthcareOrganizationStatus.Active());
    }
}