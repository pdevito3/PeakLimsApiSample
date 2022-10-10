namespace PeakLims.UnitTests.UnitTests.Domain.Tests;

using PeakLims.SharedTestHelpers.Fakes.Test;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateTestTests
{
    private readonly Faker _faker;

    public UpdateTestTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_test()
    {
        // Arrange
        var fakeTest = FakeTest.Generate();
        var updatedTest = new FakeTestForUpdateDto().Generate();
        
        // Act
        fakeTest.Update(updatedTest);

        // Assert
        fakeTest.TestNumber.Should().Be(updatedTest.TestNumber);
        fakeTest.TestCode.Should().Be(updatedTest.TestCode);
        fakeTest.TestName.Should().Be(updatedTest.TestName);
        fakeTest.Methodology.Should().Be(updatedTest.Methodology);
        fakeTest.Platform.Should().Be(updatedTest.Platform);
        fakeTest.Version.Should().Be(updatedTest.Version);
        fakeTest.Status.Should().Be(fakeTest.Status);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeTest = FakeTest.Generate();
        var updatedTest = new FakeTestForUpdateDto().Generate();
        fakeTest.DomainEvents.Clear();
        
        // Act
        fakeTest.Update(updatedTest);

        // Assert
        fakeTest.DomainEvents.Count.Should().Be(1);
        fakeTest.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestUpdated));
    }
}