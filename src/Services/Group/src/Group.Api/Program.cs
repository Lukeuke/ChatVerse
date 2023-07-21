using System.Text;
using Chat.Domain.Models;
using Group.Application;
using Group.Domain.Data;
using Group.Domain.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GroupDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"));
});

builder.Services.AddScoped<IGroupRepository, GroupRepository>();

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

app.MapGet("/group/{id:guid}", (Guid id, IGroupRepository groupRepository) => groupRepository.GetMembers(id)).RequireAuthorization();

app.MapGet("/group/{groupId:guid}/has_member", (Guid groupId, IGroupRepository groupRepository, [FromHeader] string authorization) => groupRepository
    .CheckMember(authorization, groupId))
    .RequireAuthorization();

app.MapPut("/group", async (CreateGroupRequestDto createGroupRequestDto, IGroupRepository groupRepository, [FromHeader] string authorization) =>
{
    var model = await groupRepository.Create(authorization, createGroupRequestDto);
    return TypedResults.Created($"/group/{model.Id}", model);
}).RequireAuthorization();

app.MapPost("/group/{groupId:guid}", async (Guid groupId, AddToGroupRequestDto request, IGroupRepository groupRepository, [FromHeader] string authorization) => 
    await groupRepository.AddUserToGroup(authorization, request.UserId, groupId)).RequireAuthorization();

app.Run();