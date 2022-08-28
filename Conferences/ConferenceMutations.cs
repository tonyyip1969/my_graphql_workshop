using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Speakers;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Conferences;

[ExtendObjectType("Mutation")]
public class ConferenceMutations
{
    [UseApplicationDbContext]
    public async Task<ConferencePayload> AddConferenceAsync(
        AddConferenceInput input,
        [ScopedService] ApplicationDbContext context)
    {
        var conference = new Conference
        {
            Name = input.Name
        };

        context.Conferences.Add(conference);
        await context.SaveChangesAsync();
        return new ConferencePayload(conference);
    }

    [UseApplicationDbContext]
    public async Task<ConferencePayload> UpdateConferenceAsync(
        int id,
        string newName,
        [ScopedService] ApplicationDbContext context)
    {
        var conference = context.Conferences.FirstOrDefault(x => x.Id == id);
        if (conference == null)
            throw new GraphQLException("Conference not found!");

        conference.Name = newName;
        await context.SaveChangesAsync();
        return new ConferencePayload(conference);
    }

    [UseApplicationDbContext]
    public async Task<bool> DeleteConferenceAsync(
        int id,
        [ScopedService] ApplicationDbContext context)
    {
        var conference = await context.Conferences.FirstOrDefaultAsync(x => x.Id == id);
        if (conference == null)
            throw new GraphQLException("Conference not found!");

        context.Remove(conference);
        await context.SaveChangesAsync();
        return true;
    }


}
