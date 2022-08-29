using GraphQL.Common;
using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Sessions;
using GraphQL.Speakers;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQL.Attendees;

[ExtendObjectType("Mutation")]
public class AttendeeMutations
{
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
