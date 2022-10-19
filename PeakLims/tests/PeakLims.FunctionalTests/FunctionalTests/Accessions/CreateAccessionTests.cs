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
using Domain.Accessions.Dtos;

public class CreateAccessionTests : TestBase
{
    [Test]
    public async Task create_accession_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeAccession = new AccessionForCreationDto();

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
        var fakeAccession = new AccessionForCreationDto();

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
        var fakeAccession = new AccessionForCreationDto();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.Accessions.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeAccession);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}