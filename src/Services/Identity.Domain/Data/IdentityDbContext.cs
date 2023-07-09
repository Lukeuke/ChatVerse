using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Domain.Data;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) 
        : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}