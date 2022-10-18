namespace PeakLims.UnitTests.UnitTests.Domain.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using PeakLims.Domain.TestOrderStatuses;
using SharedTestHelpers.Fakes.Panel;
using SharedTestHelpers.Fakes.Test;

[Parallelizable]
public class CreateTestOrderTests
{
    private readonly Faker _faker;

    public CreateTestOrderTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_testOrder()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        var fakeTestOrder = FakeTestOrder.Generate(id);

        // Assert
        fakeTestOrder.Status.Should().Be(TestOrderStatus.Pending());
        fakeTestOrder.TestId.Should().Be(id);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeTestOrder = FakeTestOrder.Generate();

        // Assert
        fakeTestOrder.DomainEvents.Count.Should().Be(1);
        fakeTestOrder.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestOrderCreated));
    }

    [Test]
    public void must_have_a_valid_test()
    {
        // Arrange + Act
        var actAdd = () => TestOrder.Create(null);

        // Assert
        actAdd.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"Invalid Test.");
    }

    [Test]
    public void must_have_a_valid_testId()
    {
        // Arrange + Act
        var actAdd = () => TestOrder.Create(Guid.Empty);

        // Assert
        actAdd.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"Invalid Test Id.");
    }

    [Test]
    public void must_have_a_valid_test_and_panel()
    {
        // Arrange + Act
        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Build();
        var panel = FakePanelBuilder
            .Initialize()
            .WithMockPanelRepository()
            .WithMockTestOrderRepository()
            .Build();
        var actAddWithTest = () => TestOrder.Create(test, null);
        var actAddWithPanel = () => TestOrder.Create(null, panel);

        // Assert
        actAddWithTest.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"Invalid Panel.");
        actAddWithPanel.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"Invalid Test.");
    }

    [Test]
    public void must_have_a_valid_testId_and_panelId()
    {
        // Arrange + Act
        var actAddWithTest = () => TestOrder.Create(Guid.NewGuid(), Guid.Empty);
        var actAddWithPanel = () => TestOrder.Create(Guid.Empty, Guid.NewGuid());

        // Assert
        actAddWithTest.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"Invalid Panel Id.");
        actAddWithPanel.Should().Throw<SharedKernel.Exceptions.ValidationException>()
            .WithMessage($"Invalid Test Id.");
    }
}