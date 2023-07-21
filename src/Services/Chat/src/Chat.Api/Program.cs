using System.Text;
using Chat.Application.GraphQL;
using Chat.Application.Interceptors;
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

builder.Services.AddHttpClient("group", (provider, client) =>
{
    client.BaseAddress = new Uri("https://localhost:7248/");
});

builder.Services
    .AddGraphQLServer()
    .AddSocketSessionInterceptor<SocketSessionInterceptor>()
    .AddQueryType<Query>()
    .AddType<MessageType>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddAuthorization()
    .AddInMemorySubscriptions();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseWebSockets();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL("/graphql");

app.MapGet("/", () => "Hello World!").RequireAuthorization();

app.Run();