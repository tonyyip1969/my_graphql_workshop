using GraphQL.Data;
using GraphQL.Types;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;
using GraphQL.Speakers;
using GraphQL.Sessions;
using GraphQL.Tracks;
using GraphQL.Conferences;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options => 
    options.UseSqlite("Data Source=conferences.db"));

builder.Services.AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<SpeakerQueries>()
        .AddTypeExtension<SessionQueries>()
        .AddTypeExtension<TrackQueries>()
        .AddTypeExtension<ConferenceQueries>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<SpeakerMutations>()
        .AddTypeExtension<SessionMutations>()
        .AddTypeExtension<TrackMutations>()
        .AddTypeExtension<ConferenceMutations>()
    .AddType<SpeakerType>()
    .AddType<AttendeeType>()
    .AddType<SessionType>()
    .AddType<TrackType>()
    .AddType<ConferenceType>()
    .EnableRelaySupport()
    .AddDataLoader<SpeakerByIdDataLoader>()
    .AddDataLoader<SessionByIdDataLoader>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseRouting();

app.UseEndpoints(endpoints => 
{ 
    endpoints.MapGraphQL(); 
});

app.Run();
