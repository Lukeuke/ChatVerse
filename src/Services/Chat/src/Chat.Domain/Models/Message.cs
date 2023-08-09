using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Models;

public class Message
{
    [Key]
    public Guid Id { get; set; }
    public string SenderId { get; set; } = null!;
    public string SenderFullName { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid GroupId { get; set; }
}