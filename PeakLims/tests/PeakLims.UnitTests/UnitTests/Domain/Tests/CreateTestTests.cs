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
using PeakLims.Domain.TestStatuses;
using ValidationException = SharedKernel.Exceptions.ValidationException;

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
    
    [Test]
    public void test_must_have_name()
    {
        // Arrange
        var testToCreate = new FakeTestForCreationDto().Generate();
        testToCreate.TestName = null;
        
        // Act
        var fakeTest = () => Test.Create(testToCreate, Mock.Of<ITestRepository>());

        // Assert
        fakeTest.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void test_must_have_code()
    {
        // Arrange
        var testToCreate = new FakeTestForCreationDto().Generate();
        testToCreate.TestCode = null;
        
        // Act
        var fakeTest = () => Test.Create(testToCreate, Mock.Of<ITestRepository>());

        // Assert
        fakeTest.Should().Throw<FluentValidation.ValidationException>();
    }
    
    [Test]
    public void test_must_have_version_greater_than_or_equal_to_zero()
    {
        // Arrange
        var testToCreate = new FakeTestForCreationDto().Generate();
        testToCreate.Version = -1;
        
        // Act
        var fakeTest = () => Test.Create(testToCreate, Mock.Of<ITestRepository>());

        // Assert
        fakeTest.Should().Throw<FluentValidation.ValidationException>();
    }
}