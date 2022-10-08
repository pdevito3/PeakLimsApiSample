namespace PeakLims.UnitTests.UnitTests.Domain.TestOrders.Features;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.Dtos;
using PeakLims.Domain.TestOrders.Mappings;
using PeakLims.Domain.TestOrders.Features;
using PeakLims.Domain.TestOrders.Services;
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

public class GetTestOrderListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<ITestOrderRepository> _testOrderRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetTestOrderListTests()
    {
        _testOrderRepository = new Mock<ITestOrderRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_testOrder()
    {
        //Arrange
        var fakeTestOrderOne = FakeTestOrder.Generate();
        var fakeTestOrderTwo = FakeTestOrder.Generate();
        var fakeTestOrderThree = FakeTestOrder.Generate();
        var testOrder = new List<TestOrder>();
        testOrder.Add(fakeTestOrderOne);
        testOrder.Add(fakeTestOrderTwo);
        testOrder.Add(fakeTestOrderThree);
        var mockDbData = testOrder.AsQueryable().BuildMock();
        
        var queryParameters = new TestOrderParametersDto() { PageSize = 1, PageNumber = 2 };

        _testOrderRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetTestOrderList.Query(queryParameters);
        var handler = new GetTestOrderList.Handler(_testOrderRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_testorder_list_using_Status()
    {
        //Arrange
        var fakeTestOrderOne = FakeTestOrder.Generate(new FakeTestOrderForCreationDto()
            .RuleFor(t => t.Status, _ => "alpha")
            .Generate());
        var fakeTestOrderTwo = FakeTestOrder.Generate(new FakeTestOrderForCreationDto()
            .RuleFor(t => t.Status, _ => "bravo")
            .Generate());
        var queryParameters = new TestOrderParametersDto() { Filters = $"Status == {fakeTestOrderTwo.Status}" };

        var testOrderList = new List<TestOrder>() { fakeTestOrderOne, fakeTestOrderTwo };
        var mockDbData = testOrderList.AsQueryable().BuildMock();

        _testOrderRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestOrderList.Query(queryParameters);
        var handler = new GetTestOrderList.Handler(_testOrderRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestOrderTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_testorder_by_Status()
    {
        //Arrange
        var fakeTestOrderOne = FakeTestOrder.Generate(new FakeTestOrderForCreationDto()
            .RuleFor(t => t.Status, _ => "alpha")
            .Generate());
        var fakeTestOrderTwo = FakeTestOrder.Generate(new FakeTestOrderForCreationDto()
            .RuleFor(t => t.Status, _ => "bravo")
            .Generate());
        var queryParameters = new TestOrderParametersDto() { SortOrder = "-Status" };

        var TestOrderList = new List<TestOrder>() { fakeTestOrderOne, fakeTestOrderTwo };
        var mockDbData = TestOrderList.AsQueryable().BuildMock();

        _testOrderRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetTestOrderList.Query(queryParameters);
        var handler = new GetTestOrderList.Handler(_testOrderRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestOrderTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeTestOrderOne, options =>
                options.ExcludingMissingMembers());
    }
}