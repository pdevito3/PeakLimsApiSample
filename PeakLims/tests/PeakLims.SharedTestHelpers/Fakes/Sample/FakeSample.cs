namespace PeakLims.SharedTestHelpers.Fakes.Sample;

using AutoBogus;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.Dtos;

public class FakeSample
{
    public static Sample Generate(SampleForCreationDto sampleForCreationDto)
    {
        return Sample.Create(sampleForCreationDto);
    }

    public static Sample Generate()
    {
        return Sample.Create(new FakeSampleForCreationDto().Generate());
    }
}