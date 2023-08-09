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

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

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
    var jwtPayload = JwtPayload.Deserialize(jwt);

    var id = jwtPayload.GetValueOrDefault("id");
    var user = userRepo.Get((Guid)id);

    return user;
});

app.Run();