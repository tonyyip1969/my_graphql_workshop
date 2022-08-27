using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Tags;

[ExtendObjectType("Query")]
public class TagQueries
{
    [UseApplicationDbContext]
    public Task<List<Tag>> GetTags(
        [ScopedService] ApplicationDbContext context) => context.Tags.ToListAsync();

    public Task<Tag> GetTagByIdAsync(
        [ID(nameof(Tag))] int id,
        TagByIdDataLoader dataLoader,
        CancellationToken cancellationToken) => dataLoader.LoadAsync(id, cancellationToken);

    public async Task<IEnumerable<Tag>> GetTagsByIdAsync(
        [ID(nameof(Tag))] int[] ids,
        TagByIdDataLoader dataLoader,
        CancellationToken cancellationToken) => await dataLoader.LoadAsync(ids, cancellationToken);
}
