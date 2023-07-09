using Chat.Application.GraphQL;
using Chat.Application.Services;
using Chat.Application.Types;
using Chat.Domain.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChatDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"));
});

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddGraphQL(x => SchemaBuilder.New()
    .AddServices(x)
    .AddType<GroupType>()
    .AddType<MessageType>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .Create());

builder.Services
    .AddGraphQLServer()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseWebSockets();

app.MapGraphQL("/graphql");

app.MapGet("/", () => "Hello World!");

app.Run();