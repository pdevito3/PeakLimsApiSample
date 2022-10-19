namespace PeakLims.FunctionalTests.FunctionalTests.Patients;

using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Services;

public class DeletePatientTests : TestBase
{
    [Test]
    public async Task delete_patient_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakePatient = FakePatient.Generate(new DateTimeProvider());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakePatient);

        // Act
        var route = ApiRoutes.Patients.Delete.Replace(ApiRoutes.Patients.Id, fakePatient.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_patient_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakePatient = FakePatient.Generate(new DateTimeProvider());

        await InsertAsync(fakePatient);

        // Act
        var route = ApiRoutes.Patients.Delete.Replace(ApiRoutes.Patients.Id, fakePatient.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_patient_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakePatient = FakePatient.Generate(new DateTimeProvider());
        FactoryClient.AddAuth();

        await InsertAsync(fakePatient);

        // Act
        var route = ApiRoutes.Patients.Delete.Replace(ApiRoutes.Patients.Id, fakePatient.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}