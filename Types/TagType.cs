using GraphQL.Data;
using GraphQL.DataLoader;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace GraphQL.Types;

public class TagType : ObjectType<Tag>
{
    protected override void Configure(IObjectTypeDescriptor<Tag> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode((ctx, id) => ctx.DataLoader<TagByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));
    }
}
