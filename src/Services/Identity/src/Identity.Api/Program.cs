using System.IdentityModel.Tokens.Jwt;
using Identity.Application.Helpers;
using Identity.Application.Repositories;
using Identity.Domain.Data;
using Identity.Domain.DTOs;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"));
});

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton(settings);

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/auth", ([FromServices] IUserRepository userRepo) => userRepo.GetAll());
}

app.MapPut("/auth", async (CreateUserRequestDto request, [FromServices] IUserRepository userRepo) =>
{
    var success = userRepo.Add(request);

    await success;

    if (!success.Result.Item1) return Results.BadRequest(success.Result.Item2);
    
    var usr = success.Result.Item2 as User;
    return Results.Created($"/auth/{usr!.Id}", null);
});

app.MapPost("/auth", (LoginUserRequestDto request, [FromServices] IdentityDbContext db) =>
{
    var user = db.Users.FirstOrDefault(x => x.Username == request.Username);

    if (user is null) return Results.NotFound(new {Message = $"User with {request.Username} does not exists"});
    
    if (user.PasswordHash != AuthHelper.GenerateHash(request.Password, user.Salt))
        return Results.BadRequest(new {Message = "Password is not valid"});

    var token = AuthHelper.GenerateJwt(AuthHelper.AssembleClaimsIdentity(user), settings);
    
    return Results.Ok(new
    {
        Token = token,
        Expires = DateTimeOffset.Now.AddDays(7).ToUnixTimeSeconds()
    });
});

app.MapGet("/auth/user", ([FromServices] IUserRepository userRepo, [FromHeader] string authorization) =>
{
    var jwt = authorization.Split(" ")[1];

    var handler = new JwtSecurityTokenHandler();
    var jsonToken = handler.ReadToken(jwt);
    var tokenS = jsonToken as JwtSecurityToken;

    var id = tokenS.Claims.First(x => x.Type == "id").Value;
    
    var user = userRepo.Get(Guid.Parse(id));

    return user;
});

app.MapGet("/auth/user/{id:guid}", ([FromServices] IUserRepository userRepo, [FromHeader] string authorization, Guid id) =>
{
    var user = userRepo.Get(id);
    return new { Username = user.Username };
});

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();