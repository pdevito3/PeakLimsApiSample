namespace PeakLims.SharedTestHelpers.Fakes.Sample;

using AutoBogus;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.Dtos;

public class FakeSample
{
    public static Sample Generate(ContainerlessSampleForCreationDto containerlessSampleForCreationDto)
    {
        return Sample.Create(containerlessSampleForCreationDto);
    }

    public static Sample Generate()
    {
        return Sample.Create(new FakeContainerlessSampleForCreationDto().Generate());
    }
}