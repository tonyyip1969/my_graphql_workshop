using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Attendees;

public class AttendeePayloadBase : Payload
{
	public AttendeePayloadBase(Attendee attendee)
	{
		Attendee = attendee;
	}

    protected AttendeePayloadBase(IReadOnlyList<UserError> errors)
        : base(errors)
    {

    }

    public Attendee? Attendee { get; }
}
