namespace PeakLims.UnitTests.UnitTests.Domain.Tests;

using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.AccessionStatuses;
using PeakLims.Domain.Tests.DomainEvents;
using PeakLims.Domain.TestStatuses;
using PeakLims.SharedTestHelpers.Fakes.Test;

[Parallelizable]
public class TestStateChangeTests
{
    private readonly Faker _faker;

    public TestStateChangeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_activate_test()
    {
        // Arrange
        var fakeTest = FakeTest.Generate();
        fakeTest.DomainEvents.Clear();
        
        // Act
        fakeTest.Activate();

        // Assert
        fakeTest.Status.Should().Be(TestStatus.Active());
        fakeTest.DomainEvents.Count.Should().Be(1);
        fakeTest.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestUpdated));
    }
    
    [Test]
    public void can_deactivate_test()
    {
        // Arrange
        var fakeTest = FakeTest.Generate();
        fakeTest.DomainEvents.Clear();
        
        // Act
        fakeTest.Deactivate();

        // Assert
        fakeTest.Status.Should().Be(TestStatus.Inactive());
        fakeTest.DomainEvents.Count.Should().Be(1);
        fakeTest.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestUpdated));
    }
}