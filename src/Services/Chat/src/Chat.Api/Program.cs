using Chat.Application.GraphQL;
using Chat.Application.Services;
using Chat.Application.Types;
using HotChocolate.AspNetCore;
using HotChocolate.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

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