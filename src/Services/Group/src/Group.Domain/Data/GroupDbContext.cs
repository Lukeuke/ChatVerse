using Group.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Group.Domain.Data;

public class GroupDbContext : DbContext
{
    public GroupDbContext(DbContextOptions<GroupDbContext> options) 
        : base(options)
    {
        
    }

    
    public DbSet<Member> Members { get; set; }
}