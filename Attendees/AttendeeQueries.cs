using GraphQL.Data;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Attendees;

[ExtendObjectType("Query")]
public class AttendeeQueries
{
    [UseApplicationDbContext]
    public Task<List<Attendee>> GetAttendees(
    [ScopedService] ApplicationDbContext context) => context.Attendees.ToListAsync();
}
