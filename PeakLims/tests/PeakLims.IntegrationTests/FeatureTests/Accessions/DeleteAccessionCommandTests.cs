namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

public class DeleteAccessionCommandTests : TestBase
{
    [Test]
    public async Task can_delete_accession_from_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne);

        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.PatientId, _ => fakePatientOne.Id)
            .RuleFor(a => a.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());
        await InsertAsync(fakeAccessionOne);
        var accession = await ExecuteDbContextAsync(db => db.Accessions
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionOne.Id));

        // Act
        var command = new DeleteAccession.Command(accession.Id);
        await SendAsync(command);
        var accessionResponse = await ExecuteDbContextAsync(db => db.Accessions.CountAsync(a => a.Id == accession.Id));

        // Assert
        accessionResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_accession_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteAccession.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_accession_from_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne);

        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.PatientId, _ => fakePatientOne.Id)
            .RuleFor(a => a.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());
        await InsertAsync(fakeAccessionOne);
        var accession = await ExecuteDbContextAsync(db => db.Accessions
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionOne.Id));

        // Act
        var command = new DeleteAccession.Command(accession.Id);
        await SendAsync(command);
        var deletedAccession = await ExecuteDbContextAsync(db => db.Accessions
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == accession.Id));

        // Assert
        deletedAccession?.IsDeleted.Should().BeTrue();
    }
}