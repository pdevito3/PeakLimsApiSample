namespace PeakLims.SharedTestHelpers.Fakes.Container;

using AutoBogus;
using Domain.SampleTypes;
using PeakLims.Domain.Containers;
using PeakLims.Domain.Containers.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeContainerForCreationDto : AutoFaker<ContainerForCreationDto>
{
    public FakeContainerForCreationDto()
    {
        RuleFor(x => x.UsedFor, f => f.PickRandom(SampleType.ListNames()));
    }
}