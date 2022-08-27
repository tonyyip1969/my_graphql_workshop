using GraphQL.Data;
using HotChocolate.Types.Relay;

namespace GraphQL.Attendees;

public record AddAttendeeInput(
    string FirstName,
    string LastName,
    string UserName,
    string EmailAddress,
    [ID(nameof(Session))] IReadOnlyList<int> SessionIds);
