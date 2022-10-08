namespace PeakLims.UnitTests.UnitTests.Domain.Tests.Features;

using PeakLims.SharedTestHelpers.Fakes.Test;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.Dtos;
using PeakLims.Domain.Tests.Mappings;
using PeakLims.Domain.Tests.Features;
using PeakLims.Domain.Tests.Services;
using MapsterMapper;
using FluentAssertions;
using HeimGuard;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetTestListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<ITestRepository> _testRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetTestListTests()
    {
        _testRepository = new Mock<ITestRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_test()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate();
        var fakeTestTwo = FakeTest.Generate();
        var fakeTestThree = FakeTest.Generate();
        var test = new List<Test>();
        test.Add(fakeTestOne);
        test.Add(fakeTestTwo);
        test.Add(fakeTestThree);
        var mockDbData = test.AsQueryable().BuildMock();
        
        var queryParameters = new TestParametersDto() { PageSize = 1, PageNumber = 2 };

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_test_list_using_TestNumber()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestNumber, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestNumber, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { Filters = $"TestNumber == {fakeTestTwo.TestNumber}" };

        var testList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = testList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_test_list_using_TestCode()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestCode, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestCode, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { Filters = $"TestCode == {fakeTestTwo.TestCode}" };

        var testList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = testList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_test_list_using_TestName()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestName, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestName, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { Filters = $"TestName == {fakeTestTwo.TestName}" };

        var testList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = testList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_test_list_using_Methodology()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Methodology, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Methodology, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { Filters = $"Methodology == {fakeTestTwo.Methodology}" };

        var testList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = testList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_test_list_using_Platform()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Platform, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Platform, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { Filters = $"Platform == {fakeTestTwo.Platform}" };

        var testList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = testList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_test_list_using_Version()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Version, _ => 1)
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Version, _ => 2)
            .Generate());
        var queryParameters = new TestParametersDto() { Filters = $"Version == {fakeTestTwo.Version}" };

        var testList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = testList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_test_by_TestNumber()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestNumber, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestNumber, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { SortOrder = "-TestNumber" };

        var TestList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = TestList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_test_by_TestCode()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestCode, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestCode, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { SortOrder = "-TestCode" };

        var TestList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = TestList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_test_by_TestName()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestName, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.TestName, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { SortOrder = "-TestName" };

        var TestList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = TestList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_test_by_Methodology()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Methodology, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Methodology, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { SortOrder = "-Methodology" };

        var TestList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = TestList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_test_by_Platform()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Platform, _ => "alpha")
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Platform, _ => "bravo")
            .Generate());
        var queryParameters = new TestParametersDto() { SortOrder = "-Platform" };

        var TestList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = TestList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_test_by_Version()
    {
        //Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Version, _ => 1)
            .Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto()
            .RuleFor(t => t.Version, _ => 2)
            .Generate());
        var queryParameters = new TestParametersDto() { SortOrder = "-Version" };

        var TestList = new List<Test>() { fakeTestOne, fakeTestTwo };
        var mockDbData = TestList.AsQueryable().BuildMock();

        _testRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestList.Query(queryParameters);
        var handler = new GetTestList.Handler(_testRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestOne, options =>
                options.ExcludingMissingMembers());
    }
}