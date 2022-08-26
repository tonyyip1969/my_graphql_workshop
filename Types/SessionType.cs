using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate.Types;
using HotChocolate;
using HotChocolate.Resolvers;
using GraphQL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types;

public class SessionType : ObjectType<Session>
{
    protected override void Configure(IObjectTypeDescriptor<Session> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode((ctx, id) => ctx.DataLoader<SessionByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.SessionSpeakers)
            .ResolveWith<SessionResolvers>(t => t.GetSpeakersAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .Name("speakers");

        descriptor
            .Field(t => t.SessionAttendees)
            .ResolveWith<SessionResolvers>(t => t.GetAttendeesAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .Name("attendees");

        descriptor
            .Field(t => t.Track)
            .ResolveWith<SessionResolvers>(t => t.GetTrackAsync(default!, default!, default));

        descriptor
            .Field(t => t.Conference)
            .ResolveWith<SessionResolvers>(t => t.GetConferenceAsync(default!, default!, default));

        descriptor
            .Field(t => t.TrackId)
            .ID(nameof(Track));

        descriptor
            .Field(t => t.ConferenceId)
            .ID(nameof(Conference));
    }

    private class SessionResolvers
    {
        public async Task<IEnumerable<Speaker>> GetSpeakersAsync(
            Session session,
            [ScopedService] ApplicationDbContext dbContext,
            SpeakerByIdDataLoader speakerById,
            CancellationToken cancellationToken)
        {
            int[] speakerIds = await dbContext.Sessions
                .Where(s => s.Id == session.Id)
                .Include(s => s.SessionSpeakers)
                .SelectMany(s => s.SessionSpeakers.Select(t => t.SpeakerId))
                .ToArrayAsync();

            return await speakerById.LoadAsync(speakerIds, cancellationToken);
        }

        public async Task<IEnumerable<Attendee>> GetAttendeesAsync(
            Session session,
            [ScopedService] ApplicationDbContext dbContext,
            AttendeeByIdDataLoader attendeeById,
            CancellationToken cancellationToken)
        {
            int[] attendeeIds = await dbContext.Sessions
                .Where(s => s.Id == session.Id)
                .Include(session => session.SessionAttendees)
                .SelectMany(session => session.SessionAttendees.Select(t => t.AttendeeId))
                .ToArrayAsync();

            return await attendeeById.LoadAsync(attendeeIds, cancellationToken);
        }

        public async Task<Track?> GetTrackAsync(
            Session session,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken)
        {
            if (session.TrackId is null)
            {
                return null;
            }

            return await trackById.LoadAsync(session.TrackId.Value, cancellationToken);
        }

        public async Task<Conference?> GetConferenceAsync(
            Session session,
            ConferenceByIdDataLoader conferenceById,
            CancellationToken cancellationToken)
        {
            if (session.ConferenceId is null)
            {
                return null;
            }

            return await conferenceById.LoadAsync(session.ConferenceId.Value, cancellationToken);
        }

    }
}