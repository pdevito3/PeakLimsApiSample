namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions.DomainEvents;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.AccessionStatuses;
using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.PanelOrders;
using PeakLims.Domain.TestOrders;
using SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using SharedTestHelpers.Fakes.PanelOrder;
using SharedTestHelpers.Fakes.TestOrder;

[Parallelizable]
public class AccessionReadyForTestingTests
{
    private readonly Faker _faker;

    public AccessionReadyForTestingTests()
    {
        _faker = new Faker();
    }

    private static Accession CreateFakeAccessionForTransitionToReadyForTesting(Guid patientId,
        Guid healthcareOrganizationId,
        PanelOrder panelOrder = null,
        TestOrder testOrder = null,
        HealthcareOrganizationContact healthcareOrganizationContact = null
    )
    {
        var accessionToCreate = new FakeAccessionForCreationDto().Generate();
        accessionToCreate.PatientId = patientId;
        accessionToCreate.HealthcareOrganizationId = healthcareOrganizationId;

        var fakeAccession = A.Fake<Accession>(x => x.Wrapping(Accession.Create(accessionToCreate)));
        if(testOrder != null)
            A.CallTo(() => fakeAccession.TestOrders)
                .Returns(new List<TestOrder>()
                {
                    testOrder
                });
        if(panelOrder != null)
            A.CallTo(() => fakeAccession.PanelOrders)
                .Returns(new List<PanelOrder>()
                {
                    panelOrder
                });
        if(healthcareOrganizationContact != null)
            A.CallTo(() => fakeAccession.Contacts)
                .Returns(new List<HealthcareOrganizationContact>()
                {
                    healthcareOrganizationContact
                });
        return fakeAccession;
    }
    
    [Test]
    public void can_change_to_readyForTesting()
    {
        // Arrange
        var fakeAccession = CreateFakeAccessionForTransitionToReadyForTesting(Guid.NewGuid(), 
            Guid.NewGuid(),
            FakePanelOrder.Generate(),
            FakeTestOrder.Generate(),
            FakeHealthcareOrganizationContact.Generate());
        fakeAccession.DomainEvents.Clear();
        
        // Act
        fakeAccession.SetStatusToReadyForTesting();

        // Assert
        fakeAccession.Status.Should().Be(AccessionStatus.ReadyForTesting());
        fakeAccession.DomainEvents.Count.Should().Be(1);
        fakeAccession.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionUpdated));
    }

    [Test]
    public void can_not_transition_without_patient()
    {
        // Arrange
        var fakeAccession = CreateFakeAccessionForTransitionToReadyForTesting(Guid.Empty, 
            Guid.NewGuid(),
            FakePanelOrder.Generate(),
            FakeTestOrder.Generate(),
            FakeHealthcareOrganizationContact.Generate());
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting();

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_transition_without_org()
    {
        // Arrange
        var fakeAccession = CreateFakeAccessionForTransitionToReadyForTesting(Guid.NewGuid(),
            Guid.Empty, 
            FakePanelOrder.Generate(),
            FakeTestOrder.Generate(),
            FakeHealthcareOrganizationContact.Generate());
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting();

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_transition_without_panel_or_test()
    {
        // Arrange
        var fakeAccession = CreateFakeAccessionForTransitionToReadyForTesting(Guid.NewGuid(),
            Guid.NewGuid(),
            null,
            null,
             FakeHealthcareOrganizationContact.Generate());
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting();

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_transition_without_orgContact()
    {
        // Arrange
        var fakeAccession = CreateFakeAccessionForTransitionToReadyForTesting(Guid.NewGuid(),
            Guid.NewGuid(),
            FakePanelOrder.Generate(),
            FakeTestOrder.Generate());
        
        // Act
        var act = () => fakeAccession.SetStatusToReadyForTesting();

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
}