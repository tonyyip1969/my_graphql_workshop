using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Conferences;

public class ConferencePayloadBase : Payload
{
    public ConferencePayloadBase(Conference conference)
    {
        Conference = conference;
    }

    protected ConferencePayloadBase(IReadOnlyList<UserError> errors)
        : base(errors)
    {

    }

    public Conference? Conference { get; }
}
