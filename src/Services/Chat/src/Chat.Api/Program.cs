using System.Text;
using Chat.Application.GraphQL;
using Chat.Application.Services;
using Chat.Application.Types;
using Chat.Domain.Data;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChatDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"));
});

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IGroupService, GroupService>();

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton(settings);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false
    });

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<GroupType>()
    .AddType<MessageType>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddAuthorization()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseWebSockets();

app.MapGraphQL("/graphql");

app.MapGet("/", () => "Hello World!").RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.Run();