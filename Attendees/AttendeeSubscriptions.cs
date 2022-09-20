﻿using GraphQL.Data;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace GraphQL.Attendees;

[ExtendObjectType(Name = "Subscription")]
public class AttendeeSubscriptions
{
    [Subscribe(With = nameof(SubscribeToOnAttendeeCheckedInAsync))]
    public SessionAttendeeCheckIn OnAttendeeCheckedIn(
        [ID(nameof(Session))] int sessionId,
        [EventMessage] int attendeeId)
    {
        return new SessionAttendeeCheckIn(attendeeId, sessionId);
    }

    public async ValueTask<ISourceStream<int>> SubscribeToOnAttendeeCheckedInAsync(
        int sessionId,
        [Service] ITopicEventReceiver eventReceiver,
        CancellationToken cancellationToken)
    {
        return await eventReceiver.SubscribeAsync<string, int>(
            "OnAttendeeCheckedIn_" + sessionId, cancellationToken);
    }
}
