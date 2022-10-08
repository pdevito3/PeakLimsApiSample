namespace PeakLims.Databases.EntityConfigurations;

using Domain.RolePermissions;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    /// <summary>
    /// The database configuration for RolePermissions. 
    /// </summary>
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.Property(x => x.Role)
            .HasConversion(x => x.Value, x => new Role(x))
            .HasColumnName("role");
    }
}