using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GraphQL.Tracks;

[ExtendObjectType("Query")]
public class TrackQueries
{
    [UseApplicationDbContext]
    public async Task<IEnumerable<Track>> GetTracksAsync(
        [ScopedService] ApplicationDbContext context,
        CancellationToken cancellationToken) 
        => await context.Tracks.ToListAsync(cancellationToken);

    [UseApplicationDbContext]
    [UsePaging]
    public IQueryable<Track> GetPaginateTracks(
        [ScopedService] ApplicationDbContext context) 
        => context.Tracks.OrderBy(t => t.Name);

    [UseApplicationDbContext]
    public Task<Track> GetTrackByNameAsync(
        string name,
        [ScopedService] ApplicationDbContext context,
        CancellationToken cancellationToken) 
        => context.Tracks.FirstAsync(t => t.Name == name, cancellationToken: cancellationToken);

    [UseApplicationDbContext]
    public async Task<IEnumerable<Track>> GetTrackByNamesAsync(
        string[] names,
        [ScopedService] ApplicationDbContext context,
        CancellationToken cancellationToken) 
        => await context.Tracks.Where(t => names.Contains(t.Name))
                                .ToListAsync(cancellationToken: cancellationToken);

    public Task<Track> GetTrackByIdAsync(
        [ID(nameof(Track))] int id,
        TrackByIdDataLoader trackById,
        CancellationToken cancellationToken) 
        => trackById.LoadAsync(id, cancellationToken);

    public async Task<IEnumerable<Track>> GetTracksByIdAsync(
        [ID(nameof(Track))] int[] ids,
        TrackByIdDataLoader trackById,
        CancellationToken cancellationToken) 
        => await trackById.LoadAsync(ids, cancellationToken);
}
