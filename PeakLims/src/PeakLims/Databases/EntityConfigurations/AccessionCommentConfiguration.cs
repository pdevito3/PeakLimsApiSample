namespace PeakLims.Databases.EntityConfigurations;

using Domain.AccessionCommentStatuses;
using PeakLims.Domain.AccessionComments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AccessionCommentConfiguration : IEntityTypeConfiguration<AccessionComment>
{
    /// <summary>
    /// The database configuration for AccessionComments. 
    /// </summary>
    public void Configure(EntityTypeBuilder<AccessionComment> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion(x => x.Value, x => new AccessionCommentStatus(x))
            .HasColumnName("status");
    }
}