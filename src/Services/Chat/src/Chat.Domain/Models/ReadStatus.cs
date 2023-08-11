using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Models;

public class ReadStatus
{
    public int Id { get; set; }

    public required Guid MessageId { get; set; }
    public required Guid GroupId { get; set; }
    public virtual Message Message { get; set; }
    
    public required Guid SenderId { get; set; }
    [MaxLength(25)]
    public required string SenderUsername { get; set; } = null!;
}