using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.Domain.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(eb =>
        {
            eb.HasOne(x => x.Group);
        });
    }

    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
}