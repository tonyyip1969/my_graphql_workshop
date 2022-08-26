using GraphQL.Data;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader;

public class ConferenceByIdDataLoader : BatchDataLoader<int, Conference>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public ConferenceByIdDataLoader(
        IBatchScheduler batchScheduler,
        IDbContextFactory<ApplicationDbContext> dbContextFactory)
        : base(batchScheduler)
    {
        _dbContextFactory = dbContextFactory ??
            throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task<IReadOnlyDictionary<int, Conference>> LoadBatchAsync(
        IReadOnlyList<int> keys,
        CancellationToken cancellationToken)
    {
        await using ApplicationDbContext dbContext =
            _dbContextFactory.CreateDbContext();

        return await dbContext.Conferences
            .Where(s => keys.Contains(s.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);
    }
}
