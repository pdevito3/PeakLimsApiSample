namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.Domain.HealthcareOrganizations.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.HealthcareOrganizations.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class UpdateHealthcareOrganizationCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_healthcareorganization_in_db()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        var updatedHealthcareOrganizationDto = new FakeHealthcareOrganizationForUpdateDto().Generate();
        await InsertAsync(fakeHealthcareOrganizationOne);

        var healthcareOrganization = await ExecuteDbContextAsync(db => db.HealthcareOrganizations
            .FirstOrDefaultAsync(h => h.Id == fakeHealthcareOrganizationOne.Id));
        var id = healthcareOrganization.Id;

        // Act
        var command = new UpdateHealthcareOrganization.Command(id, updatedHealthcareOrganizationDto);
        await SendAsync(command);
        var updatedHealthcareOrganization = await ExecuteDbContextAsync(db => db.HealthcareOrganizations.FirstOrDefaultAsync(h => h.Id == id));

        // Assert
        updatedHealthcareOrganization.Name.Should().Be(updatedHealthcareOrganizationDto.Name);
        updatedHealthcareOrganization.Email.Should().Be(updatedHealthcareOrganizationDto.Email);
    }
}