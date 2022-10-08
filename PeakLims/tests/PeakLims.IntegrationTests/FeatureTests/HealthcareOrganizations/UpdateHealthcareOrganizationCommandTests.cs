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
        updatedHealthcareOrganization?.Name.Should().Be(updatedHealthcareOrganizationDto.Name);
        updatedHealthcareOrganization?.Email.Value.Should().Be(updatedHealthcareOrganizationDto.Email);
        updatedHealthcareOrganization?.PrimaryAddress.Line1.Should().Be(updatedHealthcareOrganizationDto.PrimaryAddress.Line1);
        updatedHealthcareOrganization?.PrimaryAddress.Line2.Should().Be(updatedHealthcareOrganizationDto.PrimaryAddress.Line2);
        updatedHealthcareOrganization?.PrimaryAddress.City.Should().Be(updatedHealthcareOrganizationDto.PrimaryAddress.City);
        updatedHealthcareOrganization?.PrimaryAddress.State.Should().Be(updatedHealthcareOrganizationDto.PrimaryAddress.State);
        updatedHealthcareOrganization?.PrimaryAddress.PostalCode.Value.Should().Be(updatedHealthcareOrganizationDto.PrimaryAddress.PostalCode);
        updatedHealthcareOrganization?.PrimaryAddress.Country.Should().Be(updatedHealthcareOrganizationDto.PrimaryAddress.Country);
    }
}