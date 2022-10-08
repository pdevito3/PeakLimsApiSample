namespace PeakLims.Databases.EntityConfigurations;

using Domain.Users;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <summary>
    /// The database configuration for UserRoles. 
    /// </summary>
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.Property(x => x.Role)
            .HasConversion(x => x.Value, x => new Role(x))
            .HasColumnName("role");
    }
}