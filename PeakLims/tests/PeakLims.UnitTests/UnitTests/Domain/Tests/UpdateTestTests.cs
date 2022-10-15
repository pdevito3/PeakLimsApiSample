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

        // Act
        fakeTest.Update(updatedTest, Mock.Of<ITestRepository>());

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
        
        // Act
        fakeTest.Update(updatedTest, Mock.Of<ITestRepository>());

        // Assert
        fakeTest.DomainEvents.Count.Should().Be(1);
        fakeTest.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(TestUpdated));
    }
    
    [Test]
    public void test_must_have_name()
    {
        // Arrange + Act
        var fakeTest = new FakeTestBuilder()
            .WithMockRepository()
            .Build();
        var updatedTest = new FakeTestForUpdateDto().Generate();
        updatedTest.TestName = null;
        
        // Act
        var act = () => fakeTest.Update(updatedTest, Mock.Of<ITestRepository>());

        // Assert
        act.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void test_must_have_version_greater_than_or_equal_to_zero()
    {
        // Arrange
        var fakeTest = new FakeTestBuilder()
            .WithMockRepository()
            .Build();
        var updatedTest = new FakeTestForUpdateDto().Generate();
        updatedTest.Version = -1;
        
        // Act
        var act = () => fakeTest.Update(updatedTest, Mock.Of<ITestRepository>());

        // Assert
        act.Should().Throw<FluentValidation.ValidationException>();
    }
}