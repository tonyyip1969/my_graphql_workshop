using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Tags;

public class TagPayloadBase : Payload
{
	public TagPayloadBase(Tag tag)
	{
		Tag = tag;
	}

    protected TagPayloadBase(IReadOnlyList<UserError> errors)
        : base(errors)
    {

    }

    public Tag? Tag { get; }
}
