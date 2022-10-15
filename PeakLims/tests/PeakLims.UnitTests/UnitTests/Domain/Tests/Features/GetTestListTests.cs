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
        var fakeTestOne = new FakeTestBuilder()
            .WithMockRepository()
            .Build();
        var fakeTestTwo = new FakeTestBuilder()
            .WithMockRepository()
            .Build();
        var fakeTestThree = new FakeTestBuilder()
            .WithMockRepository()
            .Build();
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
}