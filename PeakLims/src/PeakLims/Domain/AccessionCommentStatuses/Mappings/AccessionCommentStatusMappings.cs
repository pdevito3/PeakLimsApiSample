namespace PeakLims.Domain.AccessionCommentStatuses.Mappings;

using AccessionCommentStatuses;
using Mapster;

public sealed class AccessionCommentStatusMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, AccessionCommentStatus>()
            .MapWith(value => new AccessionCommentStatus(value));
        config.NewConfig<AccessionCommentStatus, string>()
            .MapWith(role => role.Value);
    }
}