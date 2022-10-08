namespace PeakLims.Domain.Containers.Mappings;

using PeakLims.Domain.Containers.Dtos;
using PeakLims.Domain.Containers;
using Mapster;

public sealed class ContainerMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Container, ContainerDto>();
        config.NewConfig<ContainerForCreationDto, Container>()
            .TwoWays();
        config.NewConfig<ContainerForUpdateDto, Container>()
            .TwoWays();
    }
}