using GraphQL.Common;
using GraphQL.Data;
using GraphQL.DataLoader;

namespace GraphQL.Attendees;

public class CheckInAttendeePayload : AttendeePayloadBase
{
    private readonly int? sessionId;

    public CheckInAttendeePayload(Attendee attendee, int sessionId) : base(attendee)
    {
        this.sessionId = sessionId;
    }

    public CheckInAttendeePayload(UserError errors) : base(new[] { errors })
    {
    }

    public async Task<Session?> GetSessionAsync(
        SessionByIdDataLoader sessionById,
        CancellationToken cancellationToken)
    {
        if (sessionId.HasValue)
        {
            return await sessionById.LoadAsync(sessionId.Value, cancellationToken);
        }

        return null;
    }
}
