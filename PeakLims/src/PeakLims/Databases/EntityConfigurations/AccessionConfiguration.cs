namespace PeakLims.Databases.EntityConfigurations;

using Domain.AccessionStatuses;
using PeakLims.Domain.Accessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resources;

public sealed class AccessionConfiguration : IEntityTypeConfiguration<Accession>
{
    /// <summary>
    /// The database configuration for Accessions. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Accession> builder)
    {
        builder.Property(o => o.AccessionNumber)
            .HasDefaultValueSql($"concat('{Consts.DatabaseSequences.AccessionNumberPrefix}', nextval('\"{Consts.DatabaseSequences.AccessionNumberPrefix}\"'))")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion(x => x.Value, x => new AccessionStatus(x));
    }
}