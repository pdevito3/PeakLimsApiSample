namespace PeakLims.IntegrationTests.FeatureTests.Patients;

using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.Domain.Patients.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.Patients.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Lifespans;
using static TestFixture;

public class UpdatePatientCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_patient_in_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        var updatedPatientDto = new FakePatientForUpdateDto().Generate();
        await InsertAsync(fakePatientOne);

        var patient = await ExecuteDbContextAsync(db => db.Patients
            .FirstOrDefaultAsync(p => p.Id == fakePatientOne.Id));
        var id = patient.Id;

        // Act
        var command = new UpdatePatient.Command(id, updatedPatientDto);
        await SendAsync(command);
        var updatedPatient = await ExecuteDbContextAsync(db => db.Patients.FirstOrDefaultAsync(p => p.Id == id));

        // Assert
        updatedPatient.FirstName.Should().Be(updatedPatientDto.FirstName);
        updatedPatient.LastName.Should().Be(updatedPatientDto.LastName);
        updatedPatient.Lifespan.Should().Be(new Lifespan((DateOnly)updatedPatientDto.Lifespan.DateOfBirth));
        updatedPatient.Race.Value.Should().Be(updatedPatientDto.Race);
        updatedPatient.Ethnicity.Value.Should().Be(updatedPatientDto.Ethnicity);
        updatedPatient.Sex.Value.Should().Be(updatedPatientDto.Sex);
        updatedPatient.InternalId.Should().Be(fakePatientOne.InternalId);
    }
}