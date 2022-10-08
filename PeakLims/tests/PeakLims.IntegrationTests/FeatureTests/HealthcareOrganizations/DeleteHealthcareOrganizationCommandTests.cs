namespace PeakLims.IntegrationTests.FeatureTests.HealthcareOrganizations;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.Domain.HealthcareOrganizations.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class DeleteHealthcareOrganizationCommandTests : TestBase
{
    [Test]
    public async Task can_delete_healthcareorganization_from_db()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);
        var healthcareOrganization = await ExecuteDbContextAsync(db => db.HealthcareOrganizations
            .FirstOrDefaultAsync(h => h.Id == fakeHealthcareOrganizationOne.Id));

        // Act
        var command = new DeleteHealthcareOrganization.Command(healthcareOrganization.Id);
        await SendAsync(command);
        var healthcareOrganizationResponse = await ExecuteDbContextAsync(db => db.HealthcareOrganizations.CountAsync(h => h.Id == healthcareOrganization.Id));

        // Assert
        healthcareOrganizationResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_healthcareorganization_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteHealthcareOrganization.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_healthcareorganization_from_db()
    {
        // Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);
        var healthcareOrganization = await ExecuteDbContextAsync(db => db.HealthcareOrganizations
            .FirstOrDefaultAsync(h => h.Id == fakeHealthcareOrganizationOne.Id));

        // Act
        var command = new DeleteHealthcareOrganization.Command(healthcareOrganization.Id);
        await SendAsync(command);
        var deletedHealthcareOrganization = await ExecuteDbContextAsync(db => db.HealthcareOrganizations
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == healthcareOrganization.Id));

        // Assert
        deletedHealthcareOrganization?.IsDeleted.Should().BeTrue();
    }
}