namespace PeakLims.Domain.Roles.Mappings;

using SharedKernel.Domain;
using Mapster;

public sealed class RoleMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, Role>()
            .MapWith(value => new Role(value));
        config.NewConfig<Role, string>()
            .MapWith(role => role.Value);
    }
}