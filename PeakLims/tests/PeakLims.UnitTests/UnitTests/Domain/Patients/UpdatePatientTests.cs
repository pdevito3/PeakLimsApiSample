namespace PeakLims.UnitTests.UnitTests.Domain.Patients;

using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.Lifespans;
using PeakLims.Domain.Patients.DomainEvents;
using PeakLims.SharedTestHelpers.Fakes.Patient;

[Parallelizable]
public class UpdatePatientTests
{
    private readonly Faker _faker;

    public UpdatePatientTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_patient()
    {
        // Arrange
        var fakePatient = FakePatient.Generate();
        var updatedPatient = new FakePatientForUpdateDto().Generate();
        
        // Act
        fakePatient.Update(updatedPatient);

        // Assert
        fakePatient.FirstName.Should().Be(updatedPatient.FirstName);
        fakePatient.LastName.Should().Be(updatedPatient.LastName);
        fakePatient.Lifespan.Should().Be(new Lifespan((DateOnly)updatedPatient.Lifespan.DateOfBirth));
        fakePatient.Race.Value.Should().Be(updatedPatient.Race);
        fakePatient.Ethnicity.Value.Should().Be(updatedPatient.Ethnicity);
        fakePatient.Sex.Value.Should().Be(updatedPatient.Sex);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakePatient = FakePatient.Generate();
        var updatedPatient = new FakePatientForUpdateDto().Generate();
        fakePatient.DomainEvents.Clear();
        
        // Act
        fakePatient.Update(updatedPatient);

        // Assert
        fakePatient.DomainEvents.Count.Should().Be(1);
        fakePatient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PatientUpdated));
    }
}