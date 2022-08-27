using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Attendees;

public class AddAttendeePayload : AttendeePayloadBase
{
    public AddAttendeePayload(UserError error)
        : base(new[] { error })
    {

    }

    public AddAttendeePayload(Attendee attendee) : base(attendee)
    {
    }

    public AddAttendeePayload(IReadOnlyList<UserError> errors) 
        : base(errors)
    {

    }
}
