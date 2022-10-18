namespace PeakLims.UnitTests.UnitTests.Domain.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using NUnit.Framework;
using PeakLims.Domain.TestOrderStatuses;
using Services;
using SharedTestHelpers.Fakes.Container;
using SharedTestHelpers.Fakes.Sample;
using SharedTestHelpers.Fakes.Test;

[Parallelizable]
public class SetStatusToReadyForTestingTests
{
    private readonly Faker _faker;

    public SetStatusToReadyForTestingTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_set_to_ready_for_testing()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var sample = FakeSample.Generate(container);
        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        var dtp = Mock.Of<DateTimeProvider>();
        var fakeTestOrder = TestOrder.Create(test);
        fakeTestOrder.SetSample(sample);
        
        // Act
        fakeTestOrder.SetStatusToReadyForTesting(dtp);

        // Assert
        fakeTestOrder.Status.Should().Be(TestOrderStatus.ReadyForTesting());
        fakeTestOrder.TatSnapshot.Should().Be(test.TurnAroundTime);
        fakeTestOrder.DueDate.Should().Be(dtp.DateOnlyUtcNow.AddDays(test.TurnAroundTime));
    }
    
    [Test]
    public void can_not_set_to_ready_for_testing_when_processing()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var sample = FakeSample.Generate(container);
        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        var dtp = Mock.Of<DateTimeProvider>();
        var fakeTestOrder = TestOrder.Create(test);
        fakeTestOrder.SetSample(sample);
        fakeTestOrder.SetStatusToReadyForTesting(dtp);
        
        // Act
        var actAdd = () => fakeTestOrder.SetStatusToReadyForTesting(dtp);

        // Assert
        actAdd.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"Test orders in a {TestOrderStatus.ReadyForTesting().Value} state can not be set to {TestOrderStatus.ReadyForTesting().Value}.");
    }

    [Test]
    public void must_have_a_sample()
    {
        // Arrange
        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        var dtp = Mock.Of<DateTimeProvider>();
        var fakeTestOrder = TestOrder.Create(test);
        
        // Act
        var actAdd = () => fakeTestOrder.SetStatusToReadyForTesting(dtp);

        // Assert
        actAdd.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"A sample is required in order to set a test order to {TestOrderStatus.ReadyForTesting().Value}.");
    }
}