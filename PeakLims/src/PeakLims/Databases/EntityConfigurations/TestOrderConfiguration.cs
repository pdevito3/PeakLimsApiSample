namespace PeakLims.Databases.EntityConfigurations;

using Domain.TestOrderCancellationReasons;
using Domain.TestOrderStatuses;
using PeakLims.Domain.TestOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class TestOrderConfiguration : IEntityTypeConfiguration<TestOrder>
{
    /// <summary>
    /// The database configuration for TestOrders. 
    /// </summary>
    public void Configure(EntityTypeBuilder<TestOrder> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion(x => x.Value, x => new TestOrderStatus(x));
        builder.Property(x => x.CancellationReason)
            .HasConversion(x => x.Value, x => new TestOrderCancellationReason(x));
    }
}