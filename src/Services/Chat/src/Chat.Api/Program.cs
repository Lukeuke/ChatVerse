using Chat.Application.GraphQL;
using Chat.Application.Services;
using Chat.Application.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddGraphQL(x => SchemaBuilder.New()
    .AddServices(x)
    .AddType<GroupType>()
    .AddType<UserType>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .Create());

var app = builder.Build();

app.MapGraphQL("/graphql");

app.MapGet("/", () => "Hello World!");

app.Run();