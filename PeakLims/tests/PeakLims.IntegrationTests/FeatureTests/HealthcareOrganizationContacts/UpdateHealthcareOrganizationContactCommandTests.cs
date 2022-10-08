namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizationContacts;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.HealthcareOrganizationContacts.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

public class UpdateHealthcareOrganizationContactCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_healthcareorganizationcontact_in_db()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());
        var updatedHealthcareOrganizationContactDto = new FakeHealthcareOrganizationContactForUpdateDto()
            .RuleFor(h => h.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate();
        await InsertAsync(fakeHealthcareOrganizationContactOne);

        var healthcareOrganizationContact = await ExecuteDbContextAsync(db => db.HealthcareOrganizationContacts
            .FirstOrDefaultAsync(h => h.Id == fakeHealthcareOrganizationContactOne.Id));
        var id = healthcareOrganizationContact.Id;

        // Act
        var command = new UpdateHealthcareOrganizationContact.Command(id, updatedHealthcareOrganizationContactDto);
        await SendAsync(command);
        var updatedHealthcareOrganizationContact = await ExecuteDbContextAsync(db => db.HealthcareOrganizationContacts.FirstOrDefaultAsync(h => h.Id == id));

        // Assert
        updatedHealthcareOrganizationContact?.Name.Should().Be(updatedHealthcareOrganizationContactDto.Name);
        updatedHealthcareOrganizationContact?.Email.Value.Should().Be(updatedHealthcareOrganizationContactDto.Email);
        updatedHealthcareOrganizationContact?.Npi.Should().Be(updatedHealthcareOrganizationContactDto.Npi);
        updatedHealthcareOrganizationContact?.HealthcareOrganizationId.Should().Be(updatedHealthcareOrganizationContactDto.HealthcareOrganizationId);
    }
}