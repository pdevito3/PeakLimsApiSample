namespace PeakLims.Databases.EntityConfigurations;

using Domain.SampleTypes;
using PeakLims.Domain.Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources;

public sealed class SampleConfiguration : IEntityTypeConfiguration<Sample>
{
    /// <summary>
    /// The database configuration for Samples. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Sample> builder)
    {
        builder.Property(x => x.ContainerId)
            .IsRequired();
        builder.Property(o => o.SampleNumber)
            .HasDefaultValueSql($"concat('{Consts.DatabaseSequences.SampleNumberPrefix}', nextval('\"{Consts.DatabaseSequences.SampleNumberPrefix}\"'))")
            .IsRequired();
        builder.Property(x => x.Type)
            .HasConversion(x => x.Value, x => new SampleType(x))
            .HasColumnName("sex");
    }
}