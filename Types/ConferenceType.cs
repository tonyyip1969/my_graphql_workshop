using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate.Types;
using HotChocolate;
using HotChocolate.Resolvers;
using GraphQL.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GraphQL.Types;

public class ConferenceType : ObjectType<Conference>
{
    protected override void Configure(IObjectTypeDescriptor<Conference> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode((ctx, id) =>
                ctx.DataLoader<ConferenceByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.Sessions)
            .ResolveWith<ConferenceResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .Name("sessions");

        descriptor
            .Field(t => t.Tracks)
            .ResolveWith<ConferenceResolvers>(t => t.GetTracksAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .Name("tracks");

    }

    private class ConferenceResolvers
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            Conference conference,
            [ScopedService] ApplicationDbContext dbContext,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken)
        {
            int[] sessionIds = await dbContext.Sessions
                .Where(s => s.Id == conference.Id)
                .Select(s => s.Id)
                .ToArrayAsync();

            return await sessionById.LoadAsync(sessionIds, cancellationToken);
        }

        public async Task<IEnumerable<Track>> GetTracksAsync(
            Conference conference,
            [ScopedService] ApplicationDbContext dbContext,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken)
        {
            int[] trackIds = await dbContext.Tracks
                .Where(s => s.Id == conference.Id)
                .Select(s => s.Id)
            .ToArrayAsync();

            return await trackById.LoadAsync(trackIds, cancellationToken);
        }
    }
}