namespace PeakLims.Domain.RolePermissions.Mappings;

using PeakLims.Domain.RolePermissions.Dtos;
using PeakLims.Domain.RolePermissions;
using Mapster;

public sealed class RolePermissionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RolePermission, RolePermissionDto>();
        config.NewConfig<RolePermissionForCreationDto, RolePermission>()
            .TwoWays();
        config.NewConfig<RolePermissionForUpdateDto, RolePermission>()
            .TwoWays();
    }
}