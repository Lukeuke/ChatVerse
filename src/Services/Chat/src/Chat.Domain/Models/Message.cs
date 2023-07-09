namespace Chat.Domain.Models;

public class Message
{
    public Guid Id { get; set; }
    public string SenderId { get; set; } = null!;
    public Guid GroupId { get; set; }
    public string Content { get; set; } = null!;
}