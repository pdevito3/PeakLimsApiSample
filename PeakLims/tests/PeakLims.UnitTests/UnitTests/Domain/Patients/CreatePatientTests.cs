namespace PeakLims.UnitTests.UnitTests.Domain.Patients;

using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.Patients.DomainEvents;
using PeakLims.SharedTestHelpers.Fakes.Patient;

[Parallelizable]
public class CreatePatientTests
{
    private readonly Faker _faker;

    public CreatePatientTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_patient()
    {
        // Arrange + Act
        var fakePatient = FakePatient.Generate();

        // Assert
        fakePatient.Should().NotBeNull();
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakePatient = FakePatient.Generate();

        // Assert
        fakePatient.DomainEvents.Count.Should().Be(1);
        fakePatient.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(PatientCreated));
    }
}