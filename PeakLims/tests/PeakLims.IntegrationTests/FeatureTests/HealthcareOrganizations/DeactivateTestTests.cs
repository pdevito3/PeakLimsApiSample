namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizations;

using System.Threading.Tasks;
using Domain.HealthcareOrganizations.Features;
using Domain.HealthcareOrganizationStatuses;
using Domain.Tests.Features;
using Domain.Tests.Services;
using Domain.TestStatuses;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PeakLims.Domain.AccessionStatuses;
using SharedTestHelpers.Fakes.HealthcareOrganization;
using SharedTestHelpers.Fakes.Test;
using static TestFixture;

public class DeactivateTestTests : TestBase
{
    [Test]
    public async Task can_deactivate_test()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate();
        fakeHealthcareOrganization.Activate();
        await InsertAsync(fakeHealthcareOrganization);

        // Act
        var command = new DeactivateHealthcareOrganization.Command(fakeHealthcareOrganization.Id);
        await SendAsync(command);
        var updatedHealthcareOrganization = await ExecuteDbContextAsync(db => db.HealthcareOrganizations.FirstOrDefaultAsync(a => a.Id == fakeHealthcareOrganization.Id));

        // Assert
        updatedHealthcareOrganization.Status.Should().Be(HealthcareOrganizationStatus.Inactive());
    }
}