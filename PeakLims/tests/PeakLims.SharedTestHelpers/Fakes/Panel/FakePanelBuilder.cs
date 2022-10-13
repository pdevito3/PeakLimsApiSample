namespace PeakLims.SharedTestHelpers.Fakes.Panel;

using AutoBogus;
using Domain.Panels.Services;
using Moq;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.Dtos;

public class FakePanelBuilder
{
    private PanelForCreationDto _panelData = new FakePanelForCreationDto().Generate();
    private IPanelRepository _panelRepository = null;
    
    public FakePanelBuilder WithDto(PanelForCreationDto panelDto)
    {
        _panelData = panelDto;
        return this;
    }
    
    public FakePanelBuilder WithRepository(IPanelRepository panelRepository)
    {
        _panelRepository = panelRepository;
        return this;
    }
    
    public FakePanelBuilder WithMockRepository(bool panelExists = false)
    {
        var mockPanelRepository = new Mock<IPanelRepository>();
        mockPanelRepository
            .Setup(x => x.Exists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(panelExists);
        
        _panelRepository = mockPanelRepository.Object;
        return this;
    }
    
    public Panel Build()
    {
        if (_panelRepository == null)
            throw new Exception("A panel repository must be provided");
        
        return Panel.Create(_panelData, _panelRepository);
    }
}