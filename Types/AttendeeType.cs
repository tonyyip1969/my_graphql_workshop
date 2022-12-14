using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate.Types;
using HotChocolate;
using HotChocolate.Resolvers;
using GraphQL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types;

public class AttendeeType : ObjectType<Attendee>
{
    protected override void Configure(IObjectTypeDescriptor<Attendee> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode((ctx, id) => ctx.DataLoader<AttendeeByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.SessionsAttendees)
            .ResolveWith<AttendeeResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .Name("sessions");

        descriptor
            .Field(t => t.ConferenceAttendees)
            .ResolveWith<AttendeeResolvers>(t => t.GetConferenceAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .Name("conferences");
    }

    private class AttendeeResolvers
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            Attendee attendee,
            [ScopedService] ApplicationDbContext dbContext,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken)
        {
            int[] sessionIds = await dbContext.Attendees
                .Where(a => a.Id == attendee.Id)
                .Include(a => a.SessionsAttendees)
                .SelectMany(a => a.SessionsAttendees.Select(t => t.SessionId))
                .ToArrayAsync();

            return await sessionById.LoadAsync(sessionIds, cancellationToken);
        }

        public async Task<IEnumerable<Conference>> GetConferenceAsync(
            Attendee attendee,
            [ScopedService] ApplicationDbContext dbContext,
            ConferenceByIdDataLoader conferenceById,
            CancellationToken cancellationToken)
        {
            int[] sessionIds = await dbContext.Attendees
                .Where(a => a.Id == attendee.Id)
                .Include(a => a.SessionsAttendees)
                .SelectMany(a => a.SessionsAttendees.Select(t => t.SessionId))
                .ToArrayAsync();

            int[] conferenceIds = await dbContext.Sessions
                .Where(s => sessionIds.Contains(s.Id))
                .Select(x => (int)x.ConferenceId)
                .ToArrayAsync();

            return await conferenceById.LoadAsync(conferenceIds, cancellationToken);
        }
    }
}
