using GraphQL.Data;
using HotChocolate.Data.Filters;

namespace GraphQL.Sessions;

public class SessionFilterInputType : FilterInputType<Session>
{
    protected override void Configure(IFilterInputTypeDescriptor<Session> descriptor)
    {
        descriptor.Ignore(x => x.Id);
        descriptor.Ignore(x => x.TrackId);
    }
}
