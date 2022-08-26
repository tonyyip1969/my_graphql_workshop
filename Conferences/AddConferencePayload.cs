using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Conferences;

public class AddConferencePayload : ConferencePayloadBase
{
    public AddConferencePayload(Conference conference) 
        : base(conference)
    {
    }

    public AddConferencePayload(IReadOnlyList<UserError> errors)
       : base(errors)
    {

    }
}
