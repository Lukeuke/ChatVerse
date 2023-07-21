using System.Text;
using Chat.Domain.Models;
using Group.Application;
using Group.Domain.Data;
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

app.Run();