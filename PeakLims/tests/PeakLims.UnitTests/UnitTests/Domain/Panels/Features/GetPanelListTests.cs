namespace PeakLims.UnitTests.UnitTests.Domain.Panels.Features;

using PeakLims.SharedTestHelpers.Fakes.Panel;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.Dtos;
using PeakLims.Domain.Panels.Mappings;
using PeakLims.Domain.Panels.Features;
using PeakLims.Domain.Panels.Services;
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

public class GetPanelListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IPanelRepository> _panelRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetPanelListTests()
    {
        _panelRepository = new Mock<IPanelRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_panel()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate();
        var fakePanelTwo = FakePanel.Generate();
        var fakePanelThree = FakePanel.Generate();
        var panel = new List<Panel>();
        panel.Add(fakePanelOne);
        panel.Add(fakePanelTwo);
        panel.Add(fakePanelThree);
        var mockDbData = panel.AsQueryable().BuildMock();
        
        var queryParameters = new PanelParametersDto() { PageSize = 1, PageNumber = 2 };

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_panel_list_using_PanelNumber()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelNumber, _ => "alpha")
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelNumber, _ => "bravo")
            .Generate());
        var queryParameters = new PanelParametersDto() { Filters = $"PanelNumber == {fakePanelTwo.PanelNumber}" };

        var panelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = panelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_panel_list_using_PanelCode()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelCode, _ => "alpha")
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelCode, _ => "bravo")
            .Generate());
        var queryParameters = new PanelParametersDto() { Filters = $"PanelCode == {fakePanelTwo.PanelCode}" };

        var panelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = panelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_panel_list_using_PanelName()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelName, _ => "alpha")
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelName, _ => "bravo")
            .Generate());
        var queryParameters = new PanelParametersDto() { Filters = $"PanelName == {fakePanelTwo.PanelName}" };

        var panelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = panelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_panel_list_using_TurnAroundTime()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.TurnAroundTime, _ => 1)
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.TurnAroundTime, _ => 2)
            .Generate());
        var queryParameters = new PanelParametersDto() { Filters = $"TurnAroundTime == {fakePanelTwo.TurnAroundTime}" };

        var panelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = panelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_panel_list_using_Type()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.Type, _ => "alpha")
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.Type, _ => "bravo")
            .Generate());
        var queryParameters = new PanelParametersDto() { Filters = $"Type == {fakePanelTwo.Type}" };

        var panelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = panelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_panel_list_using_Version()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.Version, _ => 1)
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.Version, _ => 2)
            .Generate());
        var queryParameters = new PanelParametersDto() { Filters = $"Version == {fakePanelTwo.Version}" };

        var panelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = panelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_panel_by_PanelNumber()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelNumber, _ => "alpha")
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelNumber, _ => "bravo")
            .Generate());
        var queryParameters = new PanelParametersDto() { SortOrder = "-PanelNumber" };

        var PanelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = PanelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_panel_by_PanelCode()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelCode, _ => "alpha")
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelCode, _ => "bravo")
            .Generate());
        var queryParameters = new PanelParametersDto() { SortOrder = "-PanelCode" };

        var PanelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = PanelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_panel_by_PanelName()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelName, _ => "alpha")
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.PanelName, _ => "bravo")
            .Generate());
        var queryParameters = new PanelParametersDto() { SortOrder = "-PanelName" };

        var PanelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = PanelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_panel_by_TurnAroundTime()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.TurnAroundTime, _ => 1)
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.TurnAroundTime, _ => 2)
            .Generate());
        var queryParameters = new PanelParametersDto() { SortOrder = "-TurnAroundTime" };

        var PanelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = PanelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_panel_by_Type()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.Type, _ => "alpha")
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.Type, _ => "bravo")
            .Generate());
        var queryParameters = new PanelParametersDto() { SortOrder = "-Type" };

        var PanelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = PanelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_panel_by_Version()
    {
        //Arrange
        var fakePanelOne = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.Version, _ => 1)
            .Generate());
        var fakePanelTwo = FakePanel.Generate(new FakePanelForCreationDto()
            .RuleFor(p => p.Version, _ => 2)
            .Generate());
        var queryParameters = new PanelParametersDto() { SortOrder = "-Version" };

        var PanelList = new List<Panel>() { fakePanelOne, fakePanelTwo };
        var mockDbData = PanelList.AsQueryable().BuildMock();

        _panelRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelList.Query(queryParameters);
        var handler = new GetPanelList.Handler(_panelRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelOne, options =>
                options.ExcludingMissingMembers());
    }
}