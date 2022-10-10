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
using SharedTestHelpers.Fakes.Panel;
using SharedTestHelpers.Fakes.PanelOrder;
using SharedTestHelpers.Fakes.Test;
using SharedTestHelpers.Fakes.TestOrder;

[Parallelizable]
public class ManagePanelOrderOnAccessionTests
{
    private readonly Faker _faker;

    public ManagePanelOrderOnAccessionTests()
    {
        _faker = new Faker();
    }

    [Test]
    public void can_manage_panel_order()
    {
        // Arrange
        var fakeAccession = FakeAccession.Generate();

        var test = FakeTest.GenerateActivated();
        var panel = FakePanel.Generate();
        panel.AddTest(test);
        var panelOrder = FakePanelOrder.Generate();
        panelOrder.SetPanel(panel);
        
        // Act - Add
        fakeAccession.AddPanelOrder(panelOrder);

        // Assert - Add
        fakeAccession.PanelOrders.Count.Should().Be(1);
        fakeAccession.PanelOrders.Should().ContainEquivalentOf(panelOrder);
        
        // Act - Remove
        fakeAccession.RemovePanelOrder(panelOrder);
        
        // can idempotently remove
        fakeAccession.RemovePanelOrder(panelOrder);
        fakeAccession.RemovePanelOrder(panelOrder);

        // Assert - Remove
        fakeAccession.PanelOrders.Count.Should().Be(0);
    }
    
    [Test]
    public void can_not_add_panel_order_with_inactive_test()
    {
        // Arrange
        var fakeAccession = FakeAccession.Generate();

        var test = FakeTest.Generate();
        test.Deactivate();
        var panel = FakePanel.Generate();
        panel.AddTest(test);
        var panelOrder = FakePanelOrder.Generate();
        panelOrder.SetPanel(panel);
        
        // Act
        var act = () => fakeAccession.AddPanelOrder(panelOrder);

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
}