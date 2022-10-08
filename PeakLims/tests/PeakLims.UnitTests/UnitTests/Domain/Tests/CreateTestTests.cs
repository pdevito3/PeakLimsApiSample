namespace PeakLims.UnitTests.UnitTests.Domain.Tests;

using PeakLims.SharedTestHelpers.Fakes.Test;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateTestTests
{
    private readonly Faker _faker;

    public CreateTestTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_test()
    {
        // Arrange + Act
        var testToCreate = new FakeTestForCreationDto().Generate();
        var fakeTest = FakeTest.Generate(testToCreate);

        // Assert
        fakeTest.TestNumber.Should().Be(testToCreate.TestNumber);
        fakeTest.TestCode.Should().Be(testToCreate.TestCode);
        fakeTest.TestName.Should().Be(testToCreate.TestName);
        fakeTest.Methodology.Should().Be(testToCreate.Methodology);
        fakeTest.Platform.Should().Be(testToCreate.Platform);
        fakeTest.Version.Should().Be(testToCreate.Version);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeTest = FakeTest.Generate();

        // Assert
        fakeTest.DomainEvents.Count.Should().Be(1);
        fakeTest.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestCreated));
    }
}