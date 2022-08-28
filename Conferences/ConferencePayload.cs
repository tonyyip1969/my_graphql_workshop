using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Conferences;

public class ConferencePayload : ConferencePayloadBase
{
    public ConferencePayload(Conference conference) 
        : base(conference)
    {
    }

    public ConferencePayload(IReadOnlyList<UserError> errors)
       : base(errors)
    {

    }
}
