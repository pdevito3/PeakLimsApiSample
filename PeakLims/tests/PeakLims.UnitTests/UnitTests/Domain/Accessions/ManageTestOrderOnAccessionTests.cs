namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.TestOrders;
using SharedTestHelpers.Fakes.Panel;
using SharedTestHelpers.Fakes.Test;
using SharedTestHelpers.Fakes.TestOrder;

[Parallelizable]
public class ManageTestOrderOnAccessionTests
{
    private readonly Faker _faker;

    public ManageTestOrderOnAccessionTests()
    {
        _faker = new Faker();
    }

    [Test]
    public void can_manage_test_order()
    {
        // Arrange
        var fakeAccession = FakeAccession.Generate();

        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        
        // Act - Add
        fakeAccession.AddTest(test);

        // Assert - Add
        fakeAccession.TestOrders.Count.Should().Be(1);
        fakeAccession.TestOrders
            .FirstOrDefault(x => x.TestId == test.Id)!
            .Test.Should().BeEquivalentTo(test);
        
        // Arrange - Remove
        var testOrder = fakeAccession.TestOrders.FirstOrDefault(x => x.TestId == test.Id);
        
        // Act - Can remove idempotently
        fakeAccession.RemoveTestOrder(testOrder)
            .RemoveTestOrder(testOrder)
            .RemoveTestOrder(testOrder);

        // Assert - Remove
        fakeAccession.TestOrders.Count.Should().Be(0);
    }
    
    [Test]
    public void can_not_add_test_order_with_inactive_test()
    {
        // Arrange
        var fakeAccession = FakeAccession.Generate();
        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Deactivate()
            .Build();
        var testOrder = TestOrder.Create(test);
        
        // Act
        var act = () => fakeAccession.AddTest(test);

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }

    [Test]
    public void can_not_remove_testorder_when_part_of_panel()
    {
        // Arrange
        var fakeAccession = FakeAccession.Generate();
        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        var panel = new FakePanelBuilder()
            .WithMockRepository()
            .WithTest(test)
            .Activate()
            .Build();
        fakeAccession.AddPanel(panel);
        var testOrder = fakeAccession.TestOrders.First();
        
        // Act
        var act = () => fakeAccession.RemoveTestOrder(testOrder);

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage("Test orders that are part of a panel can not be selectively removed.");
    }
}