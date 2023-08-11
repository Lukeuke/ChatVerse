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
        modelBuilder.Entity<ReadStatus>().HasOne(x => x.Message);
    }

    public DbSet<Message> Messages { get; set; }
    public DbSet<ReadStatus> ReadStatus { get; set; }
}