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
        var fakePanelOne = new FakePanelBuilder()
            .WithMockRepository()
            .Build();
        var fakePanelTwo = new FakePanelBuilder()
            .WithMockRepository()
            .Build();
        var fakePanelThree = new FakePanelBuilder()
            .WithMockRepository()
            .Build();
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
}