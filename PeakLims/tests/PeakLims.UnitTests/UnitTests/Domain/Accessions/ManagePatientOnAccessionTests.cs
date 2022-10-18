namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PeakLims.Domain.Accessions;
using Services;
using SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using SharedTestHelpers.Fakes.Patient;

[Parallelizable]
public class ManagePatientOnAccessionTests
{
    private readonly Faker _faker;

    public ManagePatientOnAccessionTests()
    {
        _faker = new Faker();
    }

    [Test]
    public void can_manage_patient()
    {
        // Arrange
        var accession = Accession.Create();
        var patient = FakePatient.Generate(Mock.Of<IDateTimeProvider>());
        
        // Act - Add
        accession.SetPatient(patient);

        // Assert - Add
        accession.Patient.Should().BeEquivalentTo(patient);
        accession.Patient.Id.Should().Be(patient.Id);
        
        // Act - Remove
        accession.RemovePatient();

        // Assert - Remove
        accession.Patient.Should().BeNull();
        accession.PatientId.Should().BeNull();
    }
}