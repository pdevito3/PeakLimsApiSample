namespace PeakLims.Domain.Lifespans.Mappings;

using Dtos;
using Mapster;

public sealed class LifespanMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LifespanDto, Lifespan>()
            .MapWith(lifespan => new Lifespan(lifespan.Age, lifespan.DateOfBirth));
        config.NewConfig<Lifespan, LifespanDto>()
            .Map(x => x.Age, y => y.Age)
            .Map(x => x.DateOfBirth, y => y.DateOfBirth)
            .Map(x => x.AgeInDays, y => y.GetAgeInDays());
        
        config.NewConfig<LifespanForCreationDto, Lifespan>()
            .MapWith(lifespan => new Lifespan(lifespan.Age, lifespan.DateOfBirth))
            .TwoWays();
        config.NewConfig<LifespanForUpdateDto, Lifespan>()
            .MapWith(lifespan => new Lifespan(lifespan.Age, lifespan.DateOfBirth))
            .TwoWays();
    }
}