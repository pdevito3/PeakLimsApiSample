namespace PeakLims.Domain.Tests.Services;

using PeakLims.Domain.Tests;
using PeakLims.Databases;
using PeakLims.Services;

public interface ITestRepository : IGenericRepository<Test>
{
    bool Exists(string testCode, int version);
}

public sealed class TestRepository : GenericRepository<Test>, ITestRepository
{
    private readonly PeakLimsDbContext _dbContext;

    public TestRepository(PeakLimsDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(string testCode, int version)
    {
        return _dbContext.Tests.Any(x => x.TestCode == testCode && x.Version == version);
    }
}
