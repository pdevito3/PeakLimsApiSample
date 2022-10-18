namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions.DomainEvents;
using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PeakLims.Domain.AccessionStatuses;
using PeakLims.Domain.TestOrderStatuses;
using Services;
using SharedTestHelpers.Fakes.Container;
using SharedTestHelpers.Fakes.HealthcareOrganization;
using SharedTestHelpers.Fakes.Patient;
using SharedTestHelpers.Fakes.Sample;
using SharedTestHelpers.Fakes.Test;

[Parallelizable]
public class AccessionReadyForTestingTests
{
    private readonly Faker _faker;

    public AccessionReadyForTestingTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_change_to_readyForTesting()
    {
        // Arrange
        var patient = FakePatient.Generate(Mock.Of<IDateTimeProvider>());
        var org = FakeHealthcareOrganization.Generate();
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .WithPatient(patient)
            .WithHealthcareOrganization(org)
            .Build();
        var container = FakeContainer.Generate();
        var sample = FakeSample.Generate(container);
        fakeAccession.TestOrders.FirstOrDefault().SetSample(sample);
        fakeAccession.DomainEvents.Clear();
        
        // Act
        fakeAccession.SetStatusToReadyForTesting(Mock.Of<IDateTimeProvider>());

        // Assert
        fakeAccession.Status.Should().Be(AccessionStatus.ReadyForTesting());
        fakeAccession.DomainEvents.Count.Should().Be(1);
        fakeAccession.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionUpdated));
    }
    
    [Test]
    public void associated_test_order_state_changed()
    {
        // Arrange
        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        var patient = FakePatient.Generate(Mock.Of<IDateTimeProvider>());
        var org = FakeHealthcareOrganization.Generate();
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .WithPatient(patient)
            .WithHealthcareOrganization(org)
            .WithTest(test)
            .Build();
        var container = FakeContainer.Generate();
        var sample = FakeSample.Generate(container);
        fakeAccession.TestOrders.FirstOrDefault().SetSample(sample);
        fakeAccession.DomainEvents.Clear();
        var dateTimeProvider = Mock.Of<IDateTimeProvider>();
        
        // Act
        fakeAccession.SetStatusToReadyForTesting(dateTimeProvider);

        // Assert
        fakeAccession.TestOrders.Count().Should().Be(1);
        var testOrder = fakeAccession.TestOrders.FirstOrDefault();
        testOrder.Status.Should().Be(TestOrderStatus.ReadyForTesting());
        testOrder.TatSnapshot.Should().Be(test.TurnAroundTime);
        testOrder.DueDate.Should().Be(dateTimeProvider.DateOnlyUtcNow.AddDays(test.TurnAroundTime));
    }

    [Test]
    public void can_not_transition_without_patient()
    {
        // Arrange
        var org = FakeHealthcareOrganization.Generate();
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .WithHealthcareOrganization(org)
            .Build();
        fakeAccession.DomainEvents.Clear();
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting(Mock.Of<IDateTimeProvider>());

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_transition_without_org()
    {
        // Arrange
        var patient = FakePatient.Generate(Mock.Of<IDateTimeProvider>());
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .WithPatient(patient)
            .Build();
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting(Mock.Of<IDateTimeProvider>());

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_transition_without_panel_or_test()
    {
        // Arrange
        var patient = FakePatient.Generate(Mock.Of<IDateTimeProvider>());
        var org = FakeHealthcareOrganization.Generate();
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .WithPatient(patient)
            .WithHealthcareOrganization(org)
            .ExcludeTestOrders()
            .Build();
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting(Mock.Of<IDateTimeProvider>());

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_transition_without_orgContact()
    {
        // Arrange
        var patient = FakePatient.Generate(Mock.Of<IDateTimeProvider>());
        var org = FakeHealthcareOrganization.Generate();
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .WithPatient(patient)
            .WithHealthcareOrganization(org)
            .ExcludeContacts()
            .Build();
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting(Mock.Of<IDateTimeProvider>());

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
}