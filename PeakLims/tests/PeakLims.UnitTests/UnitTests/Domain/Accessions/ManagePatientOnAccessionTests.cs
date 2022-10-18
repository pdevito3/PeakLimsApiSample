namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PeakLims.Domain.Accessions;
using Services;
using SharedTestHelpers.Fakes.Container;
using SharedTestHelpers.Fakes.HealthcareOrganization;
using SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using SharedTestHelpers.Fakes.Patient;
using SharedTestHelpers.Fakes.Sample;

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

    [Test]
    public void can_not_update_patient_when_processing_accession()
    {
        // Arrange
        var patient = FakePatient.Generate(Mock.Of<IDateTimeProvider>());
        var org = FakeHealthcareOrganization.Generate();
        var accession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .WithPatient(patient)
            .WithHealthcareOrganization(org)
            .Build();
        var container = FakeContainer.Generate();
        var sample = FakeSample.Generate(container);
        accession.TestOrders.FirstOrDefault().SetSample(sample);
        accession.DomainEvents.Clear();
        accession.SetStatusToReadyForTesting(Mock.Of<IDateTimeProvider>());

        var anotherPatient = FakePatient.Generate(Mock.Of<IDateTimeProvider>());
        
        // Act
        var actAdd = () => accession.SetPatient(anotherPatient);
        var actRemove = () => accession.RemovePatient();

        // Assert
        actAdd.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"This accession is processing. The patient can not be modified.");
        actRemove.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"This accession is processing. The patient can not be modified.");
    }
}