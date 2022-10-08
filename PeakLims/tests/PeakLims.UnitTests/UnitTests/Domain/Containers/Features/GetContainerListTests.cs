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

    [Test]
    public async Task can_filter_container_list_using_ContainerNumber()
    {
        //Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.ContainerNumber, _ => "alpha")
            .Generate());
        var fakeContainerTwo = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.ContainerNumber, _ => "bravo")
            .Generate());
        var queryParameters = new ContainerParametersDto() { Filters = $"ContainerNumber == {fakeContainerTwo.ContainerNumber}" };

        var containerList = new List<Container>() { fakeContainerOne, fakeContainerTwo };
        var mockDbData = containerList.AsQueryable().BuildMock();

        _containerRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetContainerList.Query(queryParameters);
        var handler = new GetContainerList.Handler(_containerRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeContainerTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_container_list_using_State()
    {
        //Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.State, _ => "alpha")
            .Generate());
        var fakeContainerTwo = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.State, _ => "bravo")
            .Generate());
        var queryParameters = new ContainerParametersDto() { Filters = $"State == {fakeContainerTwo.State}" };

        var containerList = new List<Container>() { fakeContainerOne, fakeContainerTwo };
        var mockDbData = containerList.AsQueryable().BuildMock();

        _containerRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetContainerList.Query(queryParameters);
        var handler = new GetContainerList.Handler(_containerRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeContainerTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_container_list_using_Type()
    {
        //Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.Type, _ => "alpha")
            .Generate());
        var fakeContainerTwo = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.Type, _ => "bravo")
            .Generate());
        var queryParameters = new ContainerParametersDto() { Filters = $"Type == {fakeContainerTwo.Type}" };

        var containerList = new List<Container>() { fakeContainerOne, fakeContainerTwo };
        var mockDbData = containerList.AsQueryable().BuildMock();

        _containerRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetContainerList.Query(queryParameters);
        var handler = new GetContainerList.Handler(_containerRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeContainerTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_container_by_ContainerNumber()
    {
        //Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.ContainerNumber, _ => "alpha")
            .Generate());
        var fakeContainerTwo = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.ContainerNumber, _ => "bravo")
            .Generate());
        var queryParameters = new ContainerParametersDto() { SortOrder = "-ContainerNumber" };

        var ContainerList = new List<Container>() { fakeContainerOne, fakeContainerTwo };
        var mockDbData = ContainerList.AsQueryable().BuildMock();

        _containerRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetContainerList.Query(queryParameters);
        var handler = new GetContainerList.Handler(_containerRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeContainerTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeContainerOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_container_by_State()
    {
        //Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.State, _ => "alpha")
            .Generate());
        var fakeContainerTwo = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.State, _ => "bravo")
            .Generate());
        var queryParameters = new ContainerParametersDto() { SortOrder = "-State" };

        var ContainerList = new List<Container>() { fakeContainerOne, fakeContainerTwo };
        var mockDbData = ContainerList.AsQueryable().BuildMock();

        _containerRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetContainerList.Query(queryParameters);
        var handler = new GetContainerList.Handler(_containerRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeContainerTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeContainerOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_container_by_Type()
    {
        //Arrange
        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.Type, _ => "alpha")
            .Generate());
        var fakeContainerTwo = FakeContainer.Generate(new FakeContainerForCreationDto()
            .RuleFor(c => c.Type, _ => "bravo")
            .Generate());
        var queryParameters = new ContainerParametersDto() { SortOrder = "-Type" };

        var ContainerList = new List<Container>() { fakeContainerOne, fakeContainerTwo };
        var mockDbData = ContainerList.AsQueryable().BuildMock();

        _containerRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetContainerList.Query(queryParameters);
        var handler = new GetContainerList.Handler(_containerRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeContainerTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeContainerOne, options =>
                options.ExcludingMissingMembers());
    }
}