using System.Text;
using Chat.Domain.Models;
using Group.Application;
using Group.Domain.Data;
using Group.Domain.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GroupDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"));
});

builder.Services.AddScoped<IGroupRepository, GroupRepository>();

builder.Services.AddCors();

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

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/group/{id:guid}", (Guid id, IGroupRepository groupRepository) =>
{
    var json = JsonConvert.SerializeObject(groupRepository.GetMembers(id), Formatting.Indented, new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    });

    return json;

}).RequireAuthorization();

app.MapGet("/groups/me", ([FromHeader] string authorization, IGroupRepository groupRepository) => 
    groupRepository.GetAllForUser(authorization)).RequireAuthorization();

app.MapGet("/group/{groupId:guid}/has_member", (Guid groupId, IGroupRepository groupRepository, [FromHeader] string authorization) => groupRepository
    .CheckMember(authorization, groupId))
    .RequireAuthorization();

app.MapPut("/group", async (CreateGroupRequestDto createGroupRequestDto, IGroupRepository groupRepository, [FromHeader] string authorization) =>
{
    var model = await groupRepository.Create(authorization, createGroupRequestDto);
    return TypedResults.Created($"/group/{model.Id}", model);
}).RequireAuthorization();

app.MapPost("/group/{groupId:guid}", async (Guid groupId, [FromBody] AddToGroupRequestDto request, IGroupRepository groupRepository, [FromHeader] string authorization) =>
{
    var (success, content) = await groupRepository.AddUserToGroup(authorization, request.UserId, groupId);
    
    if (success)
    {
        return Results.Ok(content);
    }

    return Results.BadRequest(content);
    
}).RequireAuthorization();

app.MapDelete("/group/{groupId:guid}", async (Guid groupId, [FromBody] AddToGroupRequestDto request,
    IGroupRepository groupRepository, [FromHeader] string authorization) =>
{
    var (success, content) = await groupRepository.DeleteUserFromGroup(authorization, request.UserId, groupId);

    if (success)
    {
        return Results.NoContent();
    }

    return Results.BadRequest(content);
}).RequireAuthorization();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();