using GraphQL.Data;
using GraphQL.Types;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;
using GraphQL.Speakers;
using GraphQL.Sessions;
using GraphQL.Tracks;
using GraphQL.Conferences;
using GraphQL.Attendees;
using GraphQL.Tags;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options => 
    options.UseSqlite("Data Source=conferences.db"));

builder.Services.AddGraphQLServer()
    .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<SpeakerQueries>()
        .AddTypeExtension<SessionQueries>()
        .AddTypeExtension<TrackQueries>()
        .AddTypeExtension<ConferenceQueries>()
        .AddTypeExtension<AttendeeQueries>()
        .AddTypeExtension<TagQueries>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<SpeakerMutations>()
        .AddTypeExtension<SessionMutations>()
        .AddTypeExtension<TrackMutations>()
        .AddTypeExtension<ConferenceMutations>()
        .AddTypeExtension<AttendeeMutations>()
        .AddTypeExtension<TagMutations>()
    .AddSubscriptionType(d => d.Name("Subscription"))
        .AddTypeExtension<SessionSubscriptions>()
        .AddTypeExtension<AttendeeSubscriptions>()
    .AddType<SpeakerType>()
    .AddType<AttendeeType>()
    .AddType<SessionType>()
    .AddType<TrackType>()
    .AddType<ConferenceType>()
    .AddType<TagType>()
    .EnableRelaySupport()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions()
    .AddDataLoader<SpeakerByIdDataLoader>()
    .AddDataLoader<SessionByIdDataLoader>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseWebSockets();
app.UseRouting();

app.UseEndpoints(endpoints => 
{ 
    endpoints.MapGraphQL(); 
});

app.Run();
