namespace PeakLims.SharedTestHelpers.Fakes.Panel;

using AutoBogus;
using Domain.Panels.Services;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.Dtos;

public class FakePanel
{
    public static Panel Generate(PanelForCreationDto panelForCreationDto, IPanelRepository panelRepository)
    {
        return Panel.Create(panelForCreationDto, panelRepository);
    }
    public static Panel GenerateActivated(PanelForCreationDto panelForCreationDto, IPanelRepository panelRepository)
    {
        var panel = Panel.Create(panelForCreationDto, panelRepository);
        panel.Activate();
        return panel;
    }

    public static Panel Generate(IPanelRepository panelRepository)
    {
        return Generate(new FakePanelForCreationDto().Generate(), panelRepository);
    }

    public static Panel GenerateActivated(IPanelRepository panelRepository)
    {
        var panel = Generate(new FakePanelForCreationDto().Generate(), panelRepository);
        panel.Activate();
        return panel;
    }
}