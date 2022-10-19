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

public class DeleteAccessionTests : TestBase
{
    [Test]
    public async Task delete_accession_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .ExcludeTestOrders()
            .Build();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeAccession);

        // Act
        var route = ApiRoutes.Accessions.Delete.Replace(ApiRoutes.Accessions.Id, fakeAccession.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_accession_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .ExcludeTestOrders()
            .Build();

        await InsertAsync(fakeAccession);

        // Act
        var route = ApiRoutes.Accessions.Delete.Replace(ApiRoutes.Accessions.Id, fakeAccession.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_accession_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .ExcludeTestOrders()
            .Build();
        FactoryClient.AddAuth();

        await InsertAsync(fakeAccession);

        // Act
        var route = ApiRoutes.Accessions.Delete.Replace(ApiRoutes.Accessions.Id, fakeAccession.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}