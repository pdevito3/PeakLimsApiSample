namespace PeakLims.UnitTests.UnitTests.Domain.Containers.Features;

using PeakLims.SharedTestHelpers.Fakes.Container;
using PeakLims.Domain.Containers;
using PeakLims.Domain.Containers.Dtos;
using PeakLims.Domain.Containers.Mappings;
using PeakLims.Domain.Containers.Features;
using PeakLims.Domain.Containers.Services;
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

public class GetContainerListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IContainerRepository> _containerRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetContainerListTests()
    {
        _containerRepository = new Mock<IContainerRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_container()
    {
        //Arrange
        var fakeContainerOne = FakeContainer.Generate();
        var fakeContainerTwo = FakeContainer.Generate();
        var fakeContainerThree = FakeContainer.Generate();
        var container = new List<Container>();
        container.Add(fakeContainerOne);
        container.Add(fakeContainerTwo);
        container.Add(fakeContainerThree);
        var mockDbData = container.AsQueryable().BuildMock();
        
        var queryParameters = new ContainerParametersDto() { PageSize = 1, PageNumber = 2 };

        _containerRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetContainerList.Query(queryParameters);
        var handler = new GetContainerList.Handler(_containerRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }
}