namespace PeakLims.SharedTestHelpers.Fakes.Sample;

using AutoBogus;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeSampleForUpdateDto : AutoFaker<SampleForUpdateDto>
{
    public FakeSampleForUpdateDto()
    {
        RuleFor(s => s.ParentSampleId, _ => null);
        RuleFor(s => s.ContainerId, _ => null);
        RuleFor(s => s.PatientId, _ => null);
    }
}