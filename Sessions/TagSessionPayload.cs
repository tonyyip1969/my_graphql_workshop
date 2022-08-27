using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Sessions;

public class TagSessionPayload : SessionPayloadBase
{
    public TagSessionPayload(Session session) : base(session)
    {
    }

    public TagSessionPayload(UserError error)
        : base(new[] { error })
    {
    }

    public TagSessionPayload(IReadOnlyList<UserError> errors) : base(errors)
    {
    }
}
