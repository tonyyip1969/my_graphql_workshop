using GraphQL;
using GraphQL.Data;
using GraphQL.Types;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;
using GraphQL.Speakers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options => 
    options.UseSqlite("Data Source=conferences.db"));

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<SpeakerMutations>()
    .AddType<SpeakerType>()
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
