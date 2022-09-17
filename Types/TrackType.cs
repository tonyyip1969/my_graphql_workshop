using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate.Types;
using HotChocolate;
using HotChocolate.Resolvers;
using GraphQL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types;

public class TrackType : ObjectType<Track>
{
    protected override void Configure(IObjectTypeDescriptor<Track> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode((ctx, id) =>
                ctx.DataLoader<TrackByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.Name)
            .UseUpperCase();

        descriptor
            .Field(t => t.Sessions)
            .ResolveWith<TrackResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .UsePaging<NonNullType<SessionType>>()
            .Name("sessions");

        descriptor
            .Field(t => t.Conference)
            .ResolveWith<TrackResolvers>(t => t.GetConferenceAsync(default!, default!, default));
    }

    private class TrackResolvers
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            Track track,
            [ScopedService] ApplicationDbContext dbContext,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken)
        {
            int[] sessionIds = await dbContext.Sessions
                .Where(s => s.TrackId == track.Id)
                .Select(s => s.Id)
                .ToArrayAsync();

            return await sessionById.LoadAsync(sessionIds, cancellationToken);
        }

        public async Task<Conference?> GetConferenceAsync(
            Track track,
            ConferenceByIdDataLoader conferenceById,
            CancellationToken cancellationToken)
        {
            if (track.ConferenceId is null)
            {
                return null;
            }

            return await conferenceById.LoadAsync(track.ConferenceId.Value, cancellationToken);
        }
    }
}