namespace PeakLims.Databases.EntityConfigurations;

using Domain.Addresses;
using Domain.Emails;
using Domain.HealthcareOrganizationStatuses;
using PeakLims.Domain.HealthcareOrganizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class HealthcareOrganizationConfiguration : IEntityTypeConfiguration<HealthcareOrganization>
{
    /// <summary>
    /// The database configuration for HealthcareOrganizations. 
    /// </summary>
    public void Configure(EntityTypeBuilder<HealthcareOrganization> builder)
    {
        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .HasColumnName("email");
        
        builder.OwnsOne(x => x.PrimaryAddress, opts =>
        {
            opts.Property(x => x.Line1).HasColumnName("primary_address_line1");
            opts.Property(x => x.Line2).HasColumnName("primary_address_line2");
            opts.Property(x => x.City).HasColumnName("primary_address_city");
            opts.Property(x => x.State).HasColumnName("primary_address_state");
            opts.Property(x => x.PostalCode).HasColumnName("primary_address_postal_code")
                .HasConversion(x => x.Value, x => new PostalCode(x));
            opts.Property(x => x.Country).HasColumnName("primary_address_country");
        }).Navigation(x => x.PrimaryAddress)
            .IsRequired();
        builder.Property(x => x.Status)
            .HasConversion(x => x.Value, x => new HealthcareOrganizationStatus(x));
    }
}