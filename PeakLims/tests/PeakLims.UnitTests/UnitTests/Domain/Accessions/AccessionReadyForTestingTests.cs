namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions.DomainEvents;
using Bogus;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PeakLims.Domain.AccessionStatuses;
using PeakLims.Domain.Panels.Services;
using PeakLims.Domain.TestOrderStatuses;

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
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithPatientId(Guid.NewGuid())
            .WithHealthcareOrganizationId(Guid.NewGuid())
            .Build();
        fakeAccession.DomainEvents.Clear();
        
        // Act
        fakeAccession.SetStatusToReadyForTesting();

        // Assert
        fakeAccession.Status.Should().Be(AccessionStatus.ReadyForTesting());
        fakeAccession.DomainEvents.Count.Should().Be(1);
        fakeAccession.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionUpdated));
    }
    
    [Test]
    public void associated_test_order_state_changed()
    {
        // Arrange
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithPatientId(Guid.NewGuid())
            .WithHealthcareOrganizationId(Guid.NewGuid())
            .Build();
        fakeAccession.DomainEvents.Clear();
        
        // Act
        fakeAccession.SetStatusToReadyForTesting();

        // Assert
        fakeAccession.TestOrders
            .Count(x => x.Status == TestOrderStatus.ReadyForTesting())
            .Should()
            .Be(fakeAccession.TestOrders.Count);
    }

    [Test]
    public void can_not_transition_without_patient()
    {
        // Arrange
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithPatientId(Guid.NewGuid())
            .WithHealthcareOrganizationId(Guid.NewGuid())
            .ExcludePatient()
            .Build();
        fakeAccession.DomainEvents.Clear();
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting();

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_transition_without_org()
    {
        // Arrange
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithPatientId(Guid.NewGuid())
            .WithHealthcareOrganizationId(Guid.NewGuid())
            .ExcludeOrg()
            .Build();
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting();

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_transition_without_panel_or_test()
    {
        // Arrange
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithPatientId(Guid.NewGuid())
            .WithHealthcareOrganizationId(Guid.NewGuid())
            .ExcludeTestOrders()
            .Build();
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting();

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_transition_without_orgContact()
    {
        // Arrange
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithPatientId(Guid.NewGuid())
            .WithHealthcareOrganizationId(Guid.NewGuid())
            .ExcludeContacts()
            .Build();
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting();

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
}