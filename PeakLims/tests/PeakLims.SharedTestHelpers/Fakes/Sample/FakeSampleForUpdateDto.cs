namespace PeakLims.SharedTestHelpers.Fakes.Sample;

using AutoBogus;
using Domain.Containers;
using Domain.SampleTypes;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeSampleForUpdateDto : AutoFaker<SampleForUpdateDto>
{
    public FakeSampleForUpdateDto(Container container)
    {
        RuleFor(s => s.ParentSampleId, _ => null);
        RuleFor(s => s.PatientId, _ => null);
        RuleFor(s => s.ContainerId, container.Id);
        RuleFor(x => x.Type, container.UsedFor.Value);
    }
}