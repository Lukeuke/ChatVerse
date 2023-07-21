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
    }

    public DbSet<Message> Messages { get; set; }
}