using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Tags;

public class AddTagPayload : TagPayloadBase
{
    public AddTagPayload(Tag tag) : base(tag)
    {
    }

    protected AddTagPayload(IReadOnlyList<UserError> errors) : base(errors)
    {
    }
}
