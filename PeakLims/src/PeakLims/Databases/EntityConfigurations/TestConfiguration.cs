namespace PeakLims.Databases.EntityConfigurations;

using Domain.TestStatuses;
using PeakLims.Domain.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TestConfiguration : IEntityTypeConfiguration<Test>
{
    /// <summary>
    /// The database configuration for Tests. 
    /// </summary>
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion(x => x.Value, x => new TestStatus(x));
    }
}