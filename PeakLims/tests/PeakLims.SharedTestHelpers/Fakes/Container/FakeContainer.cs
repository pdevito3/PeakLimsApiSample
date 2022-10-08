namespace PeakLims.SharedTestHelpers.Fakes.Container;

using AutoBogus;
using PeakLims.Domain.Containers;
using PeakLims.Domain.Containers.Dtos;

public class FakeContainer
{
    public static Container Generate(ContainerForCreationDto containerForCreationDto)
    {
        return Container.Create(containerForCreationDto);
    }

    public static Container Generate()
    {
        return Container.Create(new FakeContainerForCreationDto().Generate());
    }
}