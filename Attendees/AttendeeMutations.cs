using GraphQL.Common;
using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Sessions;
using GraphQL.Speakers;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Attendees;

[ExtendObjectType("Mutation")]
public class AttendeeMutations
{
    [UseApplicationDbContext]
    public async Task<RegisterAttendeePayload> RegisterAttendeeAsync(
        RegisterAttendeeInput input,
        [ScopedService] ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var attendee = new Attendee
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            UserName = input.UserName,
            EmailAddress = input.EmailAddress
        };

        context.Attendees.Add(attendee);
        await context.SaveChangesAsync(cancellationToken);
        return new RegisterAttendeePayload(attendee);
    }

    [UseApplicationDbContext]
    public async Task<CheckInAttendeePayload> CheckInAttendeeAsync(
        checkInAttendeeInput input,
        [ScopedService] ApplicationDbContext context,
        [Service] ITopicEventSender eventSender,
        CancellationToken cancellationToken)
    {
        Attendee attendee = await context.Attendees
            .FirstOrDefaultAsync(x => x.Id == input.AttendeeId, cancellationToken);

        if (attendee == null)
        {
            return new CheckInAttendeePayload(
                new UserError("Attendee not found!", "ATTENDEE_NOT_FOUND"));
        }

        attendee.SessionsAttendees.Add(
            new SessionAttendee 
            { 
                SessionId = input.SessionId 
            });

        await context.SaveChangesAsync(cancellationToken);

        await eventSender.SendAsync("OnAttendeeCheckedIn_" + input.SessionId,
            input.AttendeeId,
            cancellationToken);

        return new CheckInAttendeePayload(attendee, input.SessionId);
    }

    [UseApplicationDbContext]
    public async Task<AddAttendeePayload> AddAttendeeAsync(
        AddAttendeeInput input,
        [ScopedService] ApplicationDbContext context)
    {
        List<int> conferenceIds = new();

        if (input.SessionIds.Count == 0)
        {
            return new AddAttendeePayload(
                new UserError(
                    "No session assigned.",
                    "NO_SESSION")
                );
        }

        var attendee = new Attendee
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            UserName = input.UserName,
            EmailAddress = input.EmailAddress
        };

        foreach (var sessionId in input.SessionIds)
        {
            var session = await context.Sessions.FindAsync(sessionId);
            if (session == null)
                throw new GraphQLException("Session not found!");

            if (conferenceIds.Any(x => x == session.ConferenceId) == false)
            {
                conferenceIds.Add(session.ConferenceId);
            }

            attendee.SessionsAttendees.Add(
                new SessionAttendee
                {
                    SessionId = sessionId
                });
        }

        foreach (var conferenceId in conferenceIds)
        {
            attendee.ConferenceAttendees.Add(
                new ConferenceAttendee
                {
                    ConferenceId = conferenceId
                });
        }

        context.Attendees.Add(attendee);
        await context.SaveChangesAsync();

        return new AddAttendeePayload(attendee);
    }
}
