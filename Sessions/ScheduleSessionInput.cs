using GraphQL.Data;
using HotChocolate.Types.Relay;

namespace GraphQL.Sessions;

public record ScheduleSessionInput(
        [ID(nameof(Session))] int SessionId,
        [ID(nameof(Track))] int TrackId,
        DateTimeOffset StartTime,
        DateTimeOffset EndTime);
