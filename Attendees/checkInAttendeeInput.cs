using GraphQL.Data;
using HotChocolate.Types.Relay;

namespace GraphQL.Attendees;

public record checkInAttendeeInput(
    [ID(nameof(Session))] int SessionId,
    [ID(nameof(Attendee))] int AttendeeId);
