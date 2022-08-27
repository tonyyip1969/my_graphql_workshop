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
            attendee.SessionsAttendees.Add(
                new SessionAttendee
                {
                    SessionId = sessionId
                });
        }

        context.Attendees.Add(attendee);
        await context.SaveChangesAsync();

        return new AddAttendeePayload(attendee);
    }
}
