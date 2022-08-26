using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Extensions;
using HotChocolate.Types.Relay;
using HotChocolate.Types;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Conferences;

[ExtendObjectType("Query")]
public class ConferenceQueries
{
    [UseApplicationDbContext]
    public Task<List<Conference>> GetConferences(
        [ScopedService] ApplicationDbContext context) => context.Conferences.ToListAsync();

    public Task<Conference> GetConferenceByIdAsync(
        [ID(nameof(Conference))] int id,
        ConferenceByIdDataLoader dataLoader,
        CancellationToken cancellationToken) => dataLoader.LoadAsync(id, cancellationToken);

    public async Task<IEnumerable<Conference>> GetSpeakersByIdAsync(
        [ID(nameof(Conference))] int[] ids,
        ConferenceByIdDataLoader dataLoader,
        CancellationToken cancellationToken) => await dataLoader.LoadAsync(ids, cancellationToken);
}
