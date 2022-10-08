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

public class CreateAccessionTests : TestBase
{
    [Test]
    public async Task create_accession_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne);

        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto().Generate());
        await InsertAsync(fakeHealthcareOrganizationOne);

        var fakeAccession = new FakeAccessionForCreationDto()
            .RuleFor(a => a.PatientId, _ => fakePatientOne.Id)
            
            .RuleFor(a => a.HealthcareOrganizationId, _ => fakeHealthcareOrganizationOne.Id)
            .Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.Accessions.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeAccession);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_accession_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeAccession = new FakeAccessionForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.Accessions.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeAccession);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_accession_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeAccession = new FakeAccessionForCreationDto { }.Generate();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.Accessions.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeAccession);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}