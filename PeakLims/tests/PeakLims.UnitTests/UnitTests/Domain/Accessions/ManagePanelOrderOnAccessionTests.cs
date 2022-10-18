namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.Accessions;
using SharedTestHelpers.Fakes.Panel;
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
        var fakeAccession = Accession.Create();

        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        var panel = FakePanelBuilder.Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Activate()
            .WithTest(test)
            .Build();
        
        // Act - Add
        fakeAccession.AddPanel(panel);

        // Assert - Add
        fakeAccession.TestOrders.Count.Should().Be(1);
        
        var orderedTest = fakeAccession.TestOrders.FirstOrDefault();
        orderedTest.Test.TestCode.Should().Be(test.TestCode);
        orderedTest.AssociatedPanel.PanelCode.Should().Be(panel.PanelCode);
        
        // Act - Can remove idempotently
        fakeAccession.RemovePanel(panel)
            .RemovePanel(panel)
            .RemovePanel(panel);

        // Assert - Remove
        fakeAccession.TestOrders.Count.Should().Be(0);
    }
    
    [Test]
    public void can_not_add_inactive_panel()
    {
        // Arrange
        var fakeAccession = Accession.Create();

        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        var panel = FakePanelBuilder.Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .WithTest(test)
            .Deactivate()
            .Build();
        
        // Act
        var act = () => fakeAccession.AddPanel(panel);

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage("This panel is not active. Only active panels can be added to an accession.");
    }
    
    [Test]
    public void can_not_add_panel_order_with_inactive_test()
    {
        // Arrange
        var fakeAccession = Accession.Create();

        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Deactivate()
            .Build();
        var panel = FakePanelBuilder.Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Activate()
            .WithTest(test)
            .Build();
        
        // Act
        var act = () => fakeAccession.AddPanel(panel);

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage("This panel has one or more tests that are not active. Only active tests can be added to an accession.");
    }
}