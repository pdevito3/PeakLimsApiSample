namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.Accessions.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

public class AddAccessionCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_accession_to_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne);

        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeAccessionOne = new FakeAccessionForCreationDto()
            .RuleFor(a => a.PatientId, _ => fakePatientOne.Id)
            .RuleFor(a => a.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate();

        // Act
        var command = new AddAccession.Command(fakeAccessionOne);
        var accessionReturned = await SendAsync(command);
        var accessionCreated = await ExecuteDbContextAsync(db => db.Accessions
            .FirstOrDefaultAsync(a => a.Id == accessionReturned.Id));

        // Assert
        accessionReturned.Status.Should().Be(fakeAccessionOne.Status);
        accessionReturned.PatientId.Should().Be(fakeAccessionOne.PatientId);
        accessionReturned.HealthcareOrganizationId.Should().Be(fakeAccessionOne.HealthcareOrganizationId);

        accessionCreated.Status.Should().Be(fakeAccessionOne.Status);
        accessionCreated.PatientId.Should().Be(fakeAccessionOne.PatientId);
        accessionCreated.HealthcareOrganizationId.Should().Be(fakeAccessionOne.HealthcareOrganizationId);
    }
}