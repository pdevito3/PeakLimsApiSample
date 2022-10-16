namespace PeakLims.SharedTestHelpers.Fakes.Panel;

using AutoBogus;
using Domain.Panels.Services;
using Domain.PanelStatuses;
using Domain.TestOrders.Services;
using Domain.Tests;
using Moq;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.Dtos;

public class FakePanelBuilder :
    IPanelRepositorySetterStage,
    ITestOrderRepositorySetterStage
{
    private PanelForCreationDto _panelData = new FakePanelForCreationDto().Generate();
    private IPanelRepository _panelRepository = null;
    private ITestOrderRepository _testOrderRepository = null;
    private PanelStatus _status;
    private readonly List<Test> _tests = new List<Test>();

    private FakePanelBuilder() { }
    public static IPanelRepositorySetterStage Initialize() => new FakePanelBuilder();
    
    public FakePanelBuilder WithDto(PanelForCreationDto panelDto)
    {
        _panelData = panelDto;
        return this;
    }
    
    public ITestOrderRepositorySetterStage WithPanelRepository(IPanelRepository panelRepository)
    {
        _panelRepository = panelRepository;
        return this;
    }
    
    public ITestOrderRepositorySetterStage WithMockPanelRepository(bool panelExists = false)
    {
        var mockPanelRepository = new Mock<IPanelRepository>();
        mockPanelRepository
            .Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(panelExists);
        
        _panelRepository = mockPanelRepository.Object;
        return this;
    }
    
    public FakePanelBuilder WithTestOrderRepository(ITestOrderRepository panelRepository)
    {
        _testOrderRepository = panelRepository;
        return this;
    }
    
    public FakePanelBuilder WithMockTestOrderRepository(bool panelIsAssignedToAnAccession = false)
    {
        var mockTestOrderRepository = new Mock<ITestOrderRepository>();
        mockTestOrderRepository
            .Setup(x => x.HasPanelAssignedToAccession(It.IsAny<Panel>()))
            .Returns(panelIsAssignedToAnAccession);
        
        _testOrderRepository = mockTestOrderRepository.Object;
        return this;
    }

    public FakePanelBuilder Activate()
    {
        _status = PanelStatus.Active();
        return this;
    }

    public FakePanelBuilder WithTest(Test test)
    {
        _tests.Add(test);
        return this;
    }

    public FakePanelBuilder Deactivate()
    {
        _status = PanelStatus.Inactive();
        return this;
    }
    
    public Panel Build()
    {
        if (_panelRepository == null)
            throw new Exception("A panel repository must be provided");
        
        var panel = Panel.Create(_panelData, _panelRepository);
        foreach (var test in _tests)
        {
            panel.AddTest(test, _testOrderRepository);
        }

        if (_status == PanelStatus.Inactive())
            panel.Deactivate();
        if (_status == PanelStatus.Active())
            panel.Activate();

        return panel;
    }
}

public interface IPanelRepositorySetterStage
{
    public ITestOrderRepositorySetterStage WithPanelRepository(IPanelRepository testRepository);
    public ITestOrderRepositorySetterStage WithMockPanelRepository(bool testExists = false);
}

public interface ITestOrderRepositorySetterStage
{
    public FakePanelBuilder WithTestOrderRepository(ITestOrderRepository testRepository);
    public FakePanelBuilder WithMockTestOrderRepository(bool testExists = false);
}