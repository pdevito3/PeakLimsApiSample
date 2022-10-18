namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PeakLims.Domain.Accessions;
using Services;
using SharedTestHelpers.Fakes.Accession;
using SharedTestHelpers.Fakes.Container;
using SharedTestHelpers.Fakes.HealthcareOrganization;
using SharedTestHelpers.Fakes.Patient;
using SharedTestHelpers.Fakes.Sample;

[Parallelizable]
public class ManageHealthcareOrganizationOnAccessionTests
{
    private readonly Faker _faker;

    public ManageHealthcareOrganizationOnAccessionTests()
    {
        _faker = new Faker();
    }

    [Test]
    public void can_manage_healthcareOrganization()
    {
        // Arrange
        var accession = Accession.Create();
        var healthcareOrganization = FakeHealthcareOrganization.Generate();
        
        // Act - add
        accession.SetHealthcareOrganization(healthcareOrganization);

        // Assert - Add
        accession.HealthcareOrganization.Should().BeEquivalentTo(healthcareOrganization);
        accession.HealthcareOrganization.Id.Should().Be(healthcareOrganization.Id);
        
        // Act - remove
        accession.RemoveHealthcareOrganization();

        // Assert - Remove
        accession.HealthcareOrganization.Should().BeNull();
        accession.HealthcareOrganizationId.Should().BeNull();
    }

    [Test]
    public void can_not_add_inactive_org()
    {
        // Arrange
        var accession = Accession.Create();
        var healthcareOrganization = FakeHealthcareOrganization.Generate();
        healthcareOrganization.Deactivate();
        
        // Act - add
        var act = () => accession.SetHealthcareOrganization(healthcareOrganization);

        // Assert - Add
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage("Only active organizations can be set on an accession.");
    }

    [Test]
    public void can_not_update_org_when_processing_accession()
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

        var anotherOrg = FakeHealthcareOrganization.Generate();
        
        // Act
        var actAdd = () => accession.SetHealthcareOrganization(anotherOrg);
        var actRemove = () => accession.RemoveHealthcareOrganization();

        // Assert
        actAdd.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"This accession is processing. The organization can not be modified.");
        actRemove.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"This accession is processing. The organization can not be modified.");
    }
}