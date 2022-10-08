namespace PeakLims.Domain.Accessions.Mappings;

using PeakLims.Domain.Accessions.Dtos;
using PeakLims.Domain.Accessions;
using Mapster;

public sealed class AccessionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Accession, AccessionDto>();
        config.NewConfig<AccessionForCreationDto, Accession>()
            .TwoWays();
        config.NewConfig<AccessionForUpdateDto, Accession>()
            .TwoWays();
    }
}