using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GraphQL;

public class Query
{
    [UseApplicationDbContext]
    public Task<List<Speaker>> GetSpeakers(
        [ScopedService] ApplicationDbContext dbContext) 
        => dbContext.Speakers.ToListAsync();

    public Task<Speaker> GetSpeakerAsync(int id, 
        SpeakerByIdDataLoader dataLoader, 
        CancellationToken cancellationToken)
    {
        return dataLoader.LoadAsync(id, cancellationToken);
    }
}
