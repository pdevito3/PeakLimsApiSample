namespace PeakLims.Databases.EntityConfigurations;

using Domain.Emails;
using PeakLims.Domain.HealthcareOrganizationContacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class HealthcareOrganizationContactConfiguration : IEntityTypeConfiguration<HealthcareOrganizationContact>
{
    /// <summary>
    /// The database configuration for HealthcareOrganizationContacts. 
    /// </summary>
    public void Configure(EntityTypeBuilder<HealthcareOrganizationContact> builder)
    {
        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .HasColumnName("email");
    }
}