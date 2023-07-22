using Group.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Group.Domain.Data;

public class GroupDbContext : DbContext
{
    public GroupDbContext(DbContextOptions<GroupDbContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>().HasMany(x => x.Groups).WithMany(x => x.Members);
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Models.Group> Groups { get; set; }
}