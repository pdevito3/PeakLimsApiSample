namespace PeakLims.Databases.EntityConfigurations;

using Domain.PanelStatuses;
using PeakLims.Domain.Panels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class PanelConfiguration : IEntityTypeConfiguration<Panel>
{
    /// <summary>
    /// The database configuration for Panels. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Panel> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion(x => x.Value, x => new PanelStatus(x));
    }
}