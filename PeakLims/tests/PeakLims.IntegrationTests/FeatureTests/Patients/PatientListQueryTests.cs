namespace PeakLims.IntegrationTests.FeatureTests.Patients;

using PeakLims.Domain.Patients.Dtos;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using SharedKernel.Exceptions;
using PeakLims.Domain.Patients.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class PatientListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_patient_list()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        var queryParameters = new PatientParametersDto();

        await InsertAsync(fakePatientOne, fakePatientTwo);

        // Act
        var query = new GetPatientList.Query(queryParameters);
        var patients = await SendAsync(query);

        // Assert
        patients.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}