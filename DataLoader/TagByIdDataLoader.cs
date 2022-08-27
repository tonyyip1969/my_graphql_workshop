using GraphQL.Data;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader;

public class TagByIdDataLoader : BatchDataLoader<int, Tag>
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public TagByIdDataLoader(
        IBatchScheduler batchScheduler, 
        IDbContextFactory<ApplicationDbContext> dbContextFactory) 
        : base(batchScheduler)
    {
        _dbContextFactory = dbContextFactory ??
            throw new ArgumentNullException(nameof(dbContextFactory));
    }

    protected override async Task<IReadOnlyDictionary<int, Tag>> LoadBatchAsync(
        IReadOnlyList<int> keys, 
        CancellationToken cancellationToken)
    {
        await using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Tags
            .Where(t => keys.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

    }
}
