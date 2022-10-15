namespace PeakLims.UnitTests.UnitTests.Domain.Tests;

using PeakLims.SharedTestHelpers.Fakes.Test;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using NUnit.Framework;
using PeakLims.Domain.Tests.Services;

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
        var fakeTest = new FakeTestBuilder()
            .WithMockRepository()
            .Build();
        var updatedTest = new FakeTestForUpdateDto().Generate();
        var mockTestRepository = new Mock<ITestRepository>();
        
        // Act
        fakeTest.Update(updatedTest, mockTestRepository.Object);

        // Assert
        fakeTest.TestCode.Should().Be(fakeTest.TestCode);
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
        var fakeTest = new FakeTestBuilder()
            .WithMockRepository()
            .Build();
        var updatedTest = new FakeTestForUpdateDto().Generate();
        fakeTest.DomainEvents.Clear();
        
        var mockTestRepository = new Mock<ITestRepository>();
        
        // Act
        fakeTest.Update(updatedTest, mockTestRepository.Object);

        // Assert
        fakeTest.DomainEvents.Count.Should().Be(1);
        fakeTest.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestUpdated));
    }
}