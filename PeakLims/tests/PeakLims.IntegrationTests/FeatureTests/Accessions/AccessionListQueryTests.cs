namespace PeakLims.IntegrationTests.FeatureTests.Accessions;

using PeakLims.Domain.Accessions.Dtos;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using SharedKernel.Exceptions;
using PeakLims.Domain.Accessions.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;

public class AccessionListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_accession_list()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne, fakePatientTwo);

        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        var fakeHealthcareOrganizationTwo = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne, fakeHealthcareOrganizationTwo);

        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.PatientId, _ => fakePatientOne.Id)
            .RuleFor(a => a.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());
        var fakeAccessionTwo = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.PatientId, _ => fakePatientTwo.Id)
            .RuleFor(a => a.HealthcareOrganizationId, _ => fakeHealthcareOrganizationTwo.Id).Generate());
        var queryParameters = new AccessionParametersDto();

        await InsertAsync(fakeAccessionOne, fakeAccessionTwo);

        // Act
        var query = new GetAccessionList.Query(queryParameters);
        var accessions = await SendAsync(query);

        // Assert
        accessions.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}