using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Attendees;

public class RegisterAttendeePayload : AttendeePayloadBase
{
    public RegisterAttendeePayload(Attendee attendee) : base(attendee)
    {
    }

    protected RegisterAttendeePayload(UserError errors) : base(new[] { errors })
    {
    }
}
