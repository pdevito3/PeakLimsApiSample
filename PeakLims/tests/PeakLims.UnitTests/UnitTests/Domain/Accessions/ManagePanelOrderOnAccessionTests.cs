namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using Bogus;
using FluentAssertions;
using NUnit.Framework;
using SharedTestHelpers.Fakes.Panel;
using SharedTestHelpers.Fakes.PanelOrder;
using SharedTestHelpers.Fakes.Test;

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
        var panel = new FakePanelBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        panel.AddTest(test);
        var panelOrder = FakePanelOrder.Generate();
        panelOrder.SetPanel(panel);
        
        // Act - Add
        fakeAccession.AddPanelOrder(panelOrder);

        // Assert - Add
        fakeAccession.PanelOrders.Count.Should().Be(1);
        fakeAccession.PanelOrders.Should().ContainEquivalentOf(panelOrder);
        
        // Act - Can remove idempotently
        fakeAccession.RemovePanelOrder(panelOrder)
            .RemovePanelOrder(panelOrder)
            .RemovePanelOrder(panelOrder);

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
        var panel = new FakePanelBuilder()
            .WithMockRepository()
            .Build();
        panel.AddTest(test);
        var panelOrder = FakePanelOrder.Generate();
        panelOrder.SetPanel(panel);
        
        // Act
        var act = () => fakeAccession.AddPanelOrder(panelOrder);

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
}