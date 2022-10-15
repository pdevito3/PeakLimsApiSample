namespace PeakLims.SharedTestHelpers.Fakes.Panel;

using AutoBogus;
using PeakLims.Domain.Panels;
using PeakLims.Domain.Panels.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakePanelForUpdateDto : AutoFaker<PanelForUpdateDto>
{
    public FakePanelForUpdateDto()
    {
        RuleFor(x => x.Version, f => f.Random.Int(0));
    }
}