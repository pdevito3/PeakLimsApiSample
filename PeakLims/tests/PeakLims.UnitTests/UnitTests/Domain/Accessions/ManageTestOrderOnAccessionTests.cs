namespace PeakLims.UnitTests.UnitTests.Domain.Accessions;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using Bogus;
using FluentAssertions;
using NUnit.Framework;
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

        var test = FakeTest.GenerateActivated();
        var testOrder = FakeTestOrder.Generate();
        testOrder.SetTest(test);
        
        // Act - Add
        fakeAccession.AddTestOrder(testOrder);

        // Assert - Add
        fakeAccession.TestOrders.Count.Should().Be(1);
        fakeAccession.TestOrders.Should().ContainEquivalentOf(testOrder);
        
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

        var test = FakeTest.Generate();
        test.Deactivate();
        var testOrder = FakeTestOrder.Generate();
        testOrder.SetTest(test);
        
        // Act
        var act = () => fakeAccession.AddTestOrder(testOrder);

        // Assert
        act.Should().Throw<SharedKernel.Exceptions.ValidationException>();
    }
}