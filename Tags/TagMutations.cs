using GraphQL.Data;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQL.Tags;

[ExtendObjectType("Mutation")]
public class TagMutations
{
    [UseApplicationDbContext]
    public async Task<AddTagPayload> AddTagAsync(
        AddTagInput input,
        [ScopedService] ApplicationDbContext context)
    {
        var tag = new Tag { Name = input.Name };
        context.Tags.Add(tag);
        await context.SaveChangesAsync();

        return new AddTagPayload(tag);
    }
}
