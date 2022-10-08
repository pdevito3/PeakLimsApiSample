namespace PeakLims.FunctionalTests.FunctionalTests.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetAccessionTests : TestBase
{
    [Test]
    public async Task get_accession_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne);

        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeAccession = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.PatientId, _ => fakePatientOne.Id)
            .RuleFor(a => a.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id).Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeAccession);

        // Act
        var route = ApiRoutes.Accessions.GetRecord.Replace(ApiRoutes.Accessions.Id, fakeAccession.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_accession_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeAccession = FakeAccession.Generate(new FakeAccessionForCreationDto().Generate());

        await InsertAsync(fakeAccession);

        // Act
        var route = ApiRoutes.Accessions.GetRecord.Replace(ApiRoutes.Accessions.Id, fakeAccession.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_accession_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeAccession = FakeAccession.Generate(new FakeAccessionForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakeAccession);

        // Act
        var route = ApiRoutes.Accessions.GetRecord.Replace(ApiRoutes.Accessions.Id, fakeAccession.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}