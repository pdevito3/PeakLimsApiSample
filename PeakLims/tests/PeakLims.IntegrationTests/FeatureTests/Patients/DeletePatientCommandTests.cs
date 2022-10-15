namespace PeakLims.IntegrationTests.FeatureTests.Patients;

using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.Domain.Patients.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Services;
using static TestFixture;

public class DeletePatientCommandTests : TestBase
{
    [Test]
    public async Task can_delete_patient_from_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);
        var patient = await ExecuteDbContextAsync(db => db.Patients
            .FirstOrDefaultAsync(p => p.Id == fakePatientOne.Id));

        // Act
        var command = new DeletePatient.Command(patient.Id);
        await SendAsync(command);
        var patientResponse = await ExecuteDbContextAsync(db => db.Patients.CountAsync(p => p.Id == patient.Id));

        // Assert
        patientResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_patient_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeletePatient.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_patient_from_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);
        var patient = await ExecuteDbContextAsync(db => db.Patients
            .FirstOrDefaultAsync(p => p.Id == fakePatientOne.Id));

        // Act
        var command = new DeletePatient.Command(patient.Id);
        await SendAsync(command);
        var deletedPatient = await ExecuteDbContextAsync(db => db.Patients
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == patient.Id));

        // Assert
        deletedPatient?.IsDeleted.Should().BeTrue();
    }
}