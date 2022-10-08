namespace PeakLims.Domain.AccessionComments.Mappings;

using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Domain.AccessionComments;
using Mapster;

public sealed class AccessionCommentMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AccessionComment, AccessionCommentDto>();
        config.NewConfig<AccessionCommentForCreationDto, AccessionComment>()
            .TwoWays();
        config.NewConfig<AccessionCommentForUpdateDto, AccessionComment>()
            .TwoWays();
    }
}