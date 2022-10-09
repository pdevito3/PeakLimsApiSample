namespace PeakLims.Domain.Accessions.Services;

using Microsoft.EntityFrameworkCore;
using PeakLims.Domain.Accessions;
using PeakLims.Databases;
using PeakLims.Services;

public interface IAccessionRepository : IGenericRepository<Accession>
{
    public Task<Accession> GetAccessionForStatusChange(Guid id, CancellationToken cancellationToken);
}

public sealed class AccessionRepository : GenericRepository<Accession>, IAccessionRepository
{
    private readonly PeakLimsDbContext _dbContext;

    public AccessionRepository(PeakLimsDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Accession> GetAccessionForStatusChange(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Accessions
            .Include(x => x.PanelOrders)
            .Include(x => x.TestOrders)
            .Include(x => x.Contacts)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
