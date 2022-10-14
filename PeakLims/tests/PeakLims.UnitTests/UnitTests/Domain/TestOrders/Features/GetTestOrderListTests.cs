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

}