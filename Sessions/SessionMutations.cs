using GraphQL.Common;
using GraphQL.Data;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;

namespace GraphQL.Sessions;

[ExtendObjectType("Mutation")]
public class SessionMutations
{
    [UseApplicationDbContext]
    public async Task<AddSessionPayload> AddSessionAsync(
            AddSessionInput input,
            [ScopedService] ApplicationDbContext context,
            CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(input.Title))
        {
            return new AddSessionPayload(
                new UserError("The title cannot be empty.", "TITLE_EMPTY"));
        }

        if (input.SpeakerIds.Count == 0)
        {
            return new AddSessionPayload(
                new UserError("No speaker assigned.", "NO_SPEAKER"));
        }

        if (context.Conferences.Any(x => x.Id == input.ConferenceId) == false)
        {
            return new AddSessionPayload(
                new UserError("Conference not found", "CONFERENCE_NOT_FOUND"));
        }

        var session = new Session
        {
            Title = input.Title,
            Abstract = input.Abstract,
            ConferenceId = input.ConferenceId,
        };

        foreach (int speakerId in input.SpeakerIds)
        {
            session.SessionSpeakers.Add(new SessionSpeaker
            {
                SpeakerId = speakerId
            });
        }

        if (input.TagIds != null && input.TagIds.Any())
        {
            foreach (var tagId in input.TagIds)
            {
                session.SessionTags.Add(new SessionTag { TagId = tagId });
            }
        }

        context.Sessions.Add(session);
        await context.SaveChangesAsync(cancellationToken);

        return new AddSessionPayload(session);
    }

    [UseApplicationDbContext]
    public async Task<ScheduleSessionPayload> ScheduleSessionAsync(
        ScheduleSessionInput input,
        [ScopedService] ApplicationDbContext context,
        [Service] ITopicEventSender eventSender)
    {
        if (input.EndTime < input.StartTime)
        {
            return new ScheduleSessionPayload(
                new UserError("endTime has to be larger than startTime.", "END_TIME_INVALID"));
        }

        Session session = await context.Sessions.FindAsync(input.SessionId);

        if (session is null)
        {
            return new ScheduleSessionPayload(
                new UserError("Session not found.", "SESSION_NOT_FOUND"));
        }

        session.TrackId = input.TrackId;
        session.StartTime = input.StartTime;
        session.EndTime = input.EndTime;

        await context.SaveChangesAsync();

        await eventSender.SendAsync(
            nameof(SessionSubscriptions.OnSessionScheduledAsync), 
            session.Id);

        return new ScheduleSessionPayload(session);
    }

    [UseApplicationDbContext]
    public async Task<TagSessionPayload> TaggingSessionAsync(
        TagSessionInput input,
        [ScopedService] ApplicationDbContext context)
    {
        Session session = await context.Sessions.FindAsync(input.SessionId);

        if (session is null)
        {
            return new TagSessionPayload(
                new UserError("Session not found.", "SESSION_NOT_FOUND")
            );
        }

        session.SessionTags.Add(new SessionTag { TagId = input.TagId });
        await context.SaveChangesAsync();

        return new TagSessionPayload(session);
    }

    [UseApplicationDbContext]
    public async Task<TagSessionPayload> DeleteSessionAsync(
        DeleteSessionInput input,
        [ScopedService] ApplicationDbContext context)
    {
        Session session = await context.Sessions.FindAsync(input.sessionId);

        if (session == null)
        {
            return new TagSessionPayload(
                new UserError("Session not found.", "SESSION_NOT_FOUND"));
        }

        context.Sessions.Remove(session);
        await context.SaveChangesAsync();

        return new TagSessionPayload(session);
    }
}
