namespace PeakLims.SharedTestHelpers.Fakes.Sample;

using AutoBogus;
using Domain.SampleTypes;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeSampleForCreationDto : AutoFaker<SampleForCreationDto>
{
    public FakeSampleForCreationDto()
    {
        RuleFor(s => s.ParentSampleId, _ => null);
        RuleFor(s => s.PatientId, _ => null);
        RuleFor(x => x.Type, f => f.PickRandom(SampleType.ListNames()));
    }
}