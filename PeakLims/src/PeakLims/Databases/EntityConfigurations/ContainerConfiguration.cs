namespace PeakLims.Databases.EntityConfigurations;

using Domain.ContainerStatuses;
using Domain.SampleTypes;
using PeakLims.Domain.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ContainerConfiguration : IEntityTypeConfiguration<Container>
{
    /// <summary>
    /// The database configuration for Containers. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Container> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion(x => x.Value, x => new ContainerStatus(x));
        builder.Property(x => x.UsedFor)
            .HasConversion(x => x.Value, x => new SampleType(x));
    }
}