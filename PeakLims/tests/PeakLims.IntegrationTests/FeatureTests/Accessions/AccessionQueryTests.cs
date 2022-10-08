namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

public class AccessionQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_accession_with_accurate_props()
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

        // Act
        var query = new GetAccession.Query(fakeAccessionOne.Id);
        var accession = await SendAsync(query);

        // Assert
        accession.AccessionNumber.Should().Be(fakeAccessionOne.AccessionNumber);
        accession.Status.Should().Be(fakeAccessionOne.Status);
        accession.PatientId.Should().Be(fakeAccessionOne.PatientId);
        accession.HealthcareOrganizationId.Should().Be(fakeAccessionOne.HealthcareOrganizationId);
    }

    [Test]
    public async Task get_accession_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetAccession.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}