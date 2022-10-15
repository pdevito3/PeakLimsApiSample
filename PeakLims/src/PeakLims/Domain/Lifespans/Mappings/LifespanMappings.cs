namespace PeakLims.Domain.Lifespans.Mappings;

using Dtos;
using Mapster;
using Services;

public sealed class LifespanMappings : IRegister
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public LifespanMappings(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }
    public LifespanMappings()
    {
        _dateTimeProvider = new DateTimeProvider();
    }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LifespanDto, Lifespan>()
            .MapWith(lifespan => new Lifespan(lifespan.Age, lifespan.DateOfBirth, _dateTimeProvider));
        config.NewConfig<Lifespan, LifespanDto>()
            .Map(x => x.Age, y => y.Age)
            .Map(x => x.DateOfBirth, y => y.DateOfBirth)
            .Map(x => x.AgeInDays, y => y.GetAgeInDays(_dateTimeProvider));
        
        config.NewConfig<LifespanForCreationDto, Lifespan>()
            .MapWith(lifespan => new Lifespan(lifespan.Age, lifespan.DateOfBirth, _dateTimeProvider))
            .TwoWays();
        config.NewConfig<LifespanForUpdateDto, Lifespan>()
            .MapWith(lifespan => new Lifespan(lifespan.Age, lifespan.DateOfBirth, _dateTimeProvider))
            .TwoWays();
    }
}