namespace PeakLims.UnitTests.UnitTests.Domain.PanelOrders.Features;

using PeakLims.SharedTestHelpers.Fakes.PanelOrder;
using PeakLims.Domain.PanelOrders;
using PeakLims.Domain.PanelOrders.Dtos;
using PeakLims.Domain.PanelOrders.Mappings;
using PeakLims.Domain.PanelOrders.Features;
using PeakLims.Domain.PanelOrders.Services;
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

public class GetPanelOrderListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IPanelOrderRepository> _panelOrderRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetPanelOrderListTests()
    {
        _panelOrderRepository = new Mock<IPanelOrderRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_panelOrder()
    {
        //Arrange
        var fakePanelOrderOne = FakePanelOrder.Generate();
        var fakePanelOrderTwo = FakePanelOrder.Generate();
        var fakePanelOrderThree = FakePanelOrder.Generate();
        var panelOrder = new List<PanelOrder>();
        panelOrder.Add(fakePanelOrderOne);
        panelOrder.Add(fakePanelOrderTwo);
        panelOrder.Add(fakePanelOrderThree);
        var mockDbData = panelOrder.AsQueryable().BuildMock();
        
        var queryParameters = new PanelOrderParametersDto() { PageSize = 1, PageNumber = 2 };

        _panelOrderRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetPanelOrderList.Query(queryParameters);
        var handler = new GetPanelOrderList.Handler(_panelOrderRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_panelorder_list_using_Status()
    {
        //Arrange
        var fakePanelOrderOne = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.Status, _ => "alpha")
            .Generate());
        var fakePanelOrderTwo = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.Status, _ => "bravo")
            .Generate());
        var queryParameters = new PanelOrderParametersDto() { Filters = $"Status == {fakePanelOrderTwo.Status}" };

        var panelOrderList = new List<PanelOrder>() { fakePanelOrderOne, fakePanelOrderTwo };
        var mockDbData = panelOrderList.AsQueryable().BuildMock();

        _panelOrderRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelOrderList.Query(queryParameters);
        var handler = new GetPanelOrderList.Handler(_panelOrderRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelOrderTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_panelorder_by_Status()
    {
        //Arrange
        var fakePanelOrderOne = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.Status, _ => "alpha")
            .Generate());
        var fakePanelOrderTwo = FakePanelOrder.Generate(new FakePanelOrderForCreationDto()
            .RuleFor(p => p.Status, _ => "bravo")
            .Generate());
        var queryParameters = new PanelOrderParametersDto() { SortOrder = "-Status" };

        var PanelOrderList = new List<PanelOrder>() { fakePanelOrderOne, fakePanelOrderTwo };
        var mockDbData = PanelOrderList.AsQueryable().BuildMock();

        _panelOrderRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPanelOrderList.Query(queryParameters);
        var handler = new GetPanelOrderList.Handler(_panelOrderRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelOrderTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePanelOrderOne, options =>
                options.ExcludingMissingMembers());
    }
}