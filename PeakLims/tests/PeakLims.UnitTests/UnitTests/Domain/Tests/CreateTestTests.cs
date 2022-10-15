namespace PeakLims.UnitTests.UnitTests.Domain.Tests;

using PeakLims.SharedTestHelpers.Fakes.Test;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using PeakLims.Domain.TestStatuses;

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
        var fakeTest = new FakeTestBuilder()
            .WithDto(testToCreate)
            .WithMockRepository()
            .Build();

        // Assert
        fakeTest.TestCode.Should().Be(testToCreate.TestCode);
        fakeTest.TestName.Should().Be(testToCreate.TestName);
        fakeTest.Methodology.Should().Be(testToCreate.Methodology);
        fakeTest.Platform.Should().Be(testToCreate.Platform);
        fakeTest.Version.Should().Be(testToCreate.Version);
        fakeTest.Status.Should().Be(TestStatus.Draft());
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeTest = new FakeTestBuilder()
            .WithMockRepository()
            .Build();

        // Assert
        fakeTest.DomainEvents.Count.Should().Be(1);
        fakeTest.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestCreated));
    }
}