namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizationContacts;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using PeakLims.Domain.HealthcareOrganizationContacts.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

public class DeleteHealthcareOrganizationContactCommandTests : TestBase
{
    [Test]
    public async Task can_delete_healthcareorganizationcontact_from_db()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());
        await InsertAsync(fakeHealthcareOrganizationContactOne);
        var healthcareOrganizationContact = await ExecuteDbContextAsync(db => db.HealthcareOrganizationContacts
            .FirstOrDefaultAsync(h => h.Id == fakeHealthcareOrganizationContactOne.Id));

        // Act
        var command = new DeleteHealthcareOrganizationContact.Command(healthcareOrganizationContact.Id);
        await SendAsync(command);
        var healthcareOrganizationContactResponse = await ExecuteDbContextAsync(db => db.HealthcareOrganizationContacts.CountAsync(h => h.Id == healthcareOrganizationContact.Id));

        // Assert
        healthcareOrganizationContactResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_healthcareorganizationcontact_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteHealthcareOrganizationContact.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_healthcareorganizationcontact_from_db()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());
        await InsertAsync(fakeHealthcareOrganizationContactOne);
        var healthcareOrganizationContact = await ExecuteDbContextAsync(db => db.HealthcareOrganizationContacts
            .FirstOrDefaultAsync(h => h.Id == fakeHealthcareOrganizationContactOne.Id));

        // Act
        var command = new DeleteHealthcareOrganizationContact.Command(healthcareOrganizationContact.Id);
        await SendAsync(command);
        var deletedHealthcareOrganizationContact = await ExecuteDbContextAsync(db => db.HealthcareOrganizationContacts
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == healthcareOrganizationContact.Id));

        // Assert
        deletedHealthcareOrganizationContact?.IsDeleted.Should().BeTrue();
    }
}