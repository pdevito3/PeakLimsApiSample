namespace PeakLims.IntegrationTests.FeatureTests.Patients;

using PeakLims.SharedTestHelpers.Fakes.Patient;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Lifespans;
using PeakLims.Domain.Patients.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddPatientCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_patient_to_db()
    {
        // Arrange
        var fakePatientOne = new FakePatientForCreationDto().Generate();

        // Act
        var command = new AddPatient.Command(fakePatientOne);
        var patientReturned = await SendAsync(command);
        var patientCreated = await ExecuteDbContextAsync(db => db.Patients
            .FirstOrDefaultAsync(p => p.Id == patientReturned.Id));

        // Assert
        var expectedLifespan = new Lifespan((DateOnly)fakePatientOne.Lifespan.DateOfBirth);
        patientReturned.FirstName.Should().Be(fakePatientOne.FirstName);
        patientReturned.LastName.Should().Be(fakePatientOne.LastName);
        patientReturned.Lifespan.DateOfBirth.Should().Be(expectedLifespan.DateOfBirth);
        patientReturned.Lifespan.Age.Should().Be(expectedLifespan.Age);
        patientReturned.Race.Should().Be(fakePatientOne.Race);
        patientReturned.Ethnicity.Should().Be(fakePatientOne.Ethnicity);
        patientReturned.Sex.Should().Be(fakePatientOne.Sex);

        patientCreated.FirstName.Should().Be(fakePatientOne.FirstName);
        patientCreated.LastName.Should().Be(fakePatientOne.LastName);
        patientCreated.Lifespan.Should().Be(expectedLifespan);
        patientCreated.Race.Value.Should().Be(fakePatientOne.Race);
        patientCreated.Ethnicity.Value.Should().Be(fakePatientOne.Ethnicity);
        patientCreated.Sex.Value.Should().Be(fakePatientOne.Sex);
    }
}