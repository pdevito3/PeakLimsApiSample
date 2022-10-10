namespace PeakLims.SharedTestHelpers.Fakes.Panel;

using AutoBogus;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.Dtos;

public class FakePanel
{
    public static Panel Generate(PanelForCreationDto panelForCreationDto)
    {
        return Panel.Create(panelForCreationDto);
    }

    public static Panel Generate()
    {
        return Generate(new FakePanelForCreationDto().Generate());
    }
}