using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Speakers;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQL.Conferences;

[ExtendObjectType("Mutation")]
public class ConferenceMutations
{
    [UseApplicationDbContext]
    public async Task<AddConferencePayload> AddConferenceAsync(
        AddConferenceInput input,
        [ScopedService] ApplicationDbContext context)
    {
        var conference = new Conference
        {
            Name = input.Name
        };

        context.Conferences.Add(conference);
        await context.SaveChangesAsync();
        return new AddConferencePayload(conference);
    }
}
