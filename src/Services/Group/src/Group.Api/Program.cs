using Group.Application;
using Group.Domain.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GroupDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb"));
});

builder.Services.AddScoped<IGroupRepository, GroupRepository>();

var app = builder.Build();

// TODO: Add Authorized endpoints
app.MapGet("/group/{id:guid}", (Guid id, IGroupRepository groupRepository) => groupRepository.GetMembers(id));

app.Run();