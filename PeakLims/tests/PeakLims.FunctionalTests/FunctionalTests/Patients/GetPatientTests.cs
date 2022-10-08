namespace PeakLims.FunctionalTests.FunctionalTests.Patients;

using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetPatientTests : TestBase
{
    [Test]
    public async Task get_patient_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakePatient = FakePatient.Generate(new FakePatientForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakePatient);

        // Act
        var route = ApiRoutes.Patients.GetRecord.Replace(ApiRoutes.Patients.Id, fakePatient.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_patient_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakePatient = FakePatient.Generate(new FakePatientForCreationDto().Generate());

        await InsertAsync(fakePatient);

        // Act
        var route = ApiRoutes.Patients.GetRecord.Replace(ApiRoutes.Patients.Id, fakePatient.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_patient_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakePatient = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakePatient);

        // Act
        var route = ApiRoutes.Patients.GetRecord.Replace(ApiRoutes.Patients.Id, fakePatient.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}