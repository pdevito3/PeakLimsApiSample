namespace PeakLims.Domain.TestOrders.Services;

using PeakLims.Domain.TestOrders;
using PeakLims.Databases;
using PeakLims.Services;

public interface ITestOrderRepository : IGenericRepository<TestOrder>
{
    void CleanupOrphanedTestOrders();
}

public sealed class TestOrderRepository : GenericRepository<TestOrder>, ITestOrderRepository
{
    private readonly PeakLimsDbContext _dbContext;

    public TestOrderRepository(PeakLimsDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void CleanupOrphanedTestOrders()
    {
        var testOrders = _dbContext.TestOrders.Where(x => x.AccessionId == null).ToList();
        _dbContext.TestOrders.RemoveRange(testOrders);
    }
}
