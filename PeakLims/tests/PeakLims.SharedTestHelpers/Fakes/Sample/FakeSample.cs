namespace PeakLims.SharedTestHelpers.Fakes.Sample;

using AutoBogus;
using Domain.Containers;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.Dtos;

public class FakeSample
{
    public static Sample Generate(ContainerlessSampleForCreationDto containerlessSampleForCreationDto, Container container)
    {
        containerlessSampleForCreationDto.Type = container.UsedFor.Value;
        return Sample.Create(containerlessSampleForCreationDto, container);
    }

    public static Sample Generate(Container container)
    {
        var sampleToCreate = new FakeContainerlessSampleForCreationDto().Generate();
        sampleToCreate.Type = container.UsedFor.Value;
        return Sample.Create(sampleToCreate, container);
    }
}